import { createAsyncThunk, createEntityAdapter, createSlice, isAnyOf } from '@reduxjs/toolkit'
import { isAxiosError } from 'axios'

import jobApplicationsApi from '@web/services/jobApplicationsApi'
import type {
  CreateJobApplicationPayload,
  JobApplication,
  JobApplicationStatus,
  JobApplicationStatusChangePayload,
  JobApplicationsQueryParams,
  PaginatedJobApplicationsResponse,
  UpdateJobApplicationPayload,
} from '@web/types/jobApplications'
import { PAGE_SIZE } from '@web/types/jobApplications'
import type { RootState } from '.'

type RequestStatus = 'idle' | 'loading' | 'succeeded' | 'failed'

const jobApplicationsAdapter = createEntityAdapter<JobApplication>({
  sortComparer: (a, b) => new Date(b.dateApplied).valueOf() - new Date(a.dateApplied).valueOf(),
})

interface PaginationState {
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface JobApplicationsState
  extends ReturnType<typeof jobApplicationsAdapter.getInitialState> {
  listStatus: RequestStatus
  listError: string | null
  mutationStatus: RequestStatus
  mutationError: string | null
  deleteStatus: RequestStatus
  deleteError: string | null
  pagination: PaginationState
  currentFilters: Required<Pick<JobApplicationsQueryParams, 'page' | 'pageSize'>> &
    Omit<JobApplicationsQueryParams, 'page' | 'pageSize'>
}

const initialFilters: JobApplicationsState['currentFilters'] = {
  page: 1,
  pageSize: PAGE_SIZE,
  status: undefined,
  searchTerm: undefined,
  appliedFrom: undefined,
  appliedTo: undefined,
}

const initialPagination: PaginationState = {
  page: 1,
  pageSize: PAGE_SIZE,
  totalCount: 0,
  totalPages: 0,
  hasPreviousPage: false,
  hasNextPage: false,
}

const initialState: JobApplicationsState = {
  ...jobApplicationsAdapter.getInitialState(),
  listStatus: 'idle',
  listError: null,
  mutationStatus: 'idle',
  mutationError: null,
  deleteStatus: 'idle',
  deleteError: null,
  pagination: initialPagination,
  currentFilters: initialFilters,
}

/**
 * Merges incoming query params with the filters currently stored in state.
 *
 * @param {RootState} state Current Redux state.
 * @param {JobApplicationsQueryParams} [params] Optional override parameters.
 * @returns {JobApplicationsState['currentFilters']} Filter set to use for requests.
 */
const getFilters = (
  state: RootState,
  params?: JobApplicationsQueryParams,
): JobApplicationsState['currentFilters'] => {
  const current = state.jobApplications.currentFilters
  const merged = {
    ...current,
    ...params,
  }

  return {
    ...merged,
    page: merged.page ?? current.page ?? 1,
    pageSize: merged.pageSize ?? current.pageSize ?? PAGE_SIZE,
  }
}

/**
 * Attempts to extract a user-friendly message from thrown errors.
 *
 * @param {unknown} error Error thrown during async execution.
 * @returns {string} Message safe to display in the UI.
 */
const extractErrorMessage = (error: unknown): string => {
  if (isAxiosError(error)) {
    return (
      error.response?.data?.title ??
      error.response?.data?.detail ??
      error.message ??
      'An unexpected network error occurred.'
    )
  }

  if (error instanceof Error) {
    return error.message
  }

  return 'An unexpected error occurred.'
}

type FetchJobApplicationsResult = {
  result: PaginatedJobApplicationsResponse<JobApplication>
  filters: JobApplicationsState['currentFilters']
}

/**
 * Retrieves a paginated list of job applications using server filters.
 *
 * @param {JobApplicationsQueryParams | undefined} params Optional filter overrides.
 * @returns {Promise<FetchJobApplicationsResult>} Fulfilled payload containing items and applied filters.
 */
export const fetchJobApplications = createAsyncThunk<
  FetchJobApplicationsResult,
  JobApplicationsQueryParams | undefined,
  { state: RootState }
>('jobApplications/fetchAll', async (params, { getState, rejectWithValue }) => {
  try {
    const state = getState()
    const filters = getFilters(state, params)
    const result = await jobApplicationsApi.list(filters)

    return { result, filters }
  } catch (error) {
    return rejectWithValue(extractErrorMessage(error))
  }
})

/**
 * Retrieves and normalizes a single job application by id.
 *
 * @param {number} id Identifier of the requested job application.
 * @returns {Promise<JobApplication>} Job application entity from the API.
 */
export const fetchJobApplicationById = createAsyncThunk<
  JobApplication,
  number,
  { state: RootState }
>('jobApplications/fetchById', async (id, { rejectWithValue }) => {
  try {
    const application = await jobApplicationsApi.getById(id)
    return application
  } catch (error) {
    return rejectWithValue(extractErrorMessage(error))
  }
})

/**
 * Creates a new job application and returns the created entity.
 *
 * @param {CreateJobApplicationPayload} payload Formatted payload for creation.
 * @returns {Promise<JobApplication>} Newly created job application.
 */
export const createJobApplication = createAsyncThunk<
  JobApplication,
  CreateJobApplicationPayload
>('jobApplications/create', async (payload, { rejectWithValue }) => {
  try {
    const application = await jobApplicationsApi.create(payload)
    return application
  } catch (error) {
    return rejectWithValue(extractErrorMessage(error))
  }
})

/**
 * Updates an existing job application before re-fetching the server copy.
 *
 * @param {{ id: number; payload: UpdateJobApplicationPayload }} args Update payload together with ID.
 * @returns {Promise<JobApplication>} Updated job application fetched after the update.
 */
export const updateJobApplication = createAsyncThunk<
  JobApplication,
  { id: number; payload: UpdateJobApplicationPayload }
>('jobApplications/update', async ({ id, payload }, { rejectWithValue }) => {
  try {
    await jobApplicationsApi.update(id, payload)
    const application = await jobApplicationsApi.getById(id)
    return application
  } catch (error) {
    return rejectWithValue(extractErrorMessage(error))
  }
})

/**
 * Updates only the status of a specific job application.
 *
 * @param {{ id: number; payload: JobApplicationStatusChangePayload }} args Identifier and payload to send.
 * @returns {Promise<JobApplication>} Updated job application returned from the API.
 */
export const changeJobApplicationStatus = createAsyncThunk<
  JobApplication,
  { id: number; payload: JobApplicationStatusChangePayload }
>('jobApplications/changeStatus', async ({ id, payload }, { rejectWithValue }) => {
  try {
    await jobApplicationsApi.changeStatus(id, payload)
    const application = await jobApplicationsApi.getById(id)
    return application
  } catch (error) {
    return rejectWithValue(extractErrorMessage(error))
  }
})

/**
 * Deletes a job application identified by id.
 *
 * @param {number} id Identifier of the job application to remove.
 * @returns {Promise<number>} Identifier of the deleted application.
 */
export const deleteJobApplication = createAsyncThunk<number, number>(
  'jobApplications/delete',
  async (id, { rejectWithValue }) => {
    try {
      await jobApplicationsApi.delete(id)
      return id
    } catch (error) {
      return rejectWithValue(extractErrorMessage(error))
    }
  },
)

const jobApplicationsSlice = createSlice({
  name: 'jobApplications',
  initialState,
  reducers: {
    /**
     * Merges the provided filter fragment into the persisted filter state.
     *
     * @param {JobApplicationsState} state Current slice state.
     * @param {{ payload: Partial<JobApplicationsState['currentFilters']> }} action Action containing filter overrides.
     */
    setFilters(
      state,
      action: {
        payload: Partial<JobApplicationsState['currentFilters']>
      },
    ) {
      state.currentFilters = {
        ...state.currentFilters,
        ...action.payload,
      }
    },
    /**
     * Updates the current page within the filter state.
     *
     * @param {JobApplicationsState} state Current slice state.
     * @param {{ payload: number }} action Action containing the new page.
     */
    setPage(state, action: { payload: number }) {
      state.currentFilters.page = action.payload
    },
    /**
     * Updates the current page size within the filter state.
     *
     * @param {JobApplicationsState} state Current slice state.
     * @param {{ payload: number }} action Action containing the new page size.
     */
    setPageSize(state, action: { payload: number }) {
      state.currentFilters.pageSize = action.payload
    },
    /**
     * Stores the status filter for subsequent fetches.
     *
     * @param {JobApplicationsState} state Current slice state.
     * @param {{ payload: JobApplicationStatus | undefined }} action Action containing the requested status filter.
     */
    setStatusFilter(state, action: { payload: JobApplicationStatus | undefined }) {
      state.currentFilters.status = action.payload
    },
    /**
     * Resets mutation status flags and errors.
     *
     * @param {JobApplicationsState} state Current slice state.
     */
    resetMutationState(state) {
      state.mutationStatus = 'idle'
      state.mutationError = null
    },
    /**
     * Resets delete status flags and errors.
     *
     * @param {JobApplicationsState} state Current slice state.
     */
    resetDeleteState(state) {
      state.deleteStatus = 'idle'
      state.deleteError = null
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchJobApplications.pending, (state) => {
        state.listStatus = 'loading'
        state.listError = null
      })
      .addCase(fetchJobApplications.fulfilled, (state, action) => {
        state.listStatus = 'succeeded'
        state.listError = null
        state.currentFilters = action.payload.filters
        state.pagination = {
          page: action.payload.result.page,
          pageSize: action.payload.result.pageSize,
          totalCount: action.payload.result.totalCount,
          totalPages: action.payload.result.totalPages,
          hasPreviousPage: action.payload.result.hasPreviousPage,
          hasNextPage: action.payload.result.hasNextPage,
        }
        jobApplicationsAdapter.setAll(state, action.payload.result.items)
      })
      .addCase(fetchJobApplications.rejected, (state, action) => {
        state.listStatus = 'failed'
        state.listError = (action.payload as string) ?? action.error.message ?? null
        jobApplicationsAdapter.removeAll(state)
      })

    builder.addCase(fetchJobApplicationById.fulfilled, (state, action) => {
      jobApplicationsAdapter.upsertOne(state, action.payload)
    })

    builder.addMatcher(
      isAnyOf(
        createJobApplication.pending,
        updateJobApplication.pending,
        changeJobApplicationStatus.pending,
      ),
      (state) => {
        state.mutationStatus = 'loading'
        state.mutationError = null
      },
    )
    builder.addMatcher(
      isAnyOf(createJobApplication.rejected, updateJobApplication.rejected, changeJobApplicationStatus.rejected),
      (state, action) => {
        state.mutationStatus = 'failed'
        state.mutationError = (action.payload as string) ?? action.error.message ?? null
      },
    )
    builder.addMatcher(
      isAnyOf(createJobApplication.fulfilled, updateJobApplication.fulfilled, changeJobApplicationStatus.fulfilled),
      (state, action) => {
        state.mutationStatus = 'succeeded'
        state.mutationError = null
        jobApplicationsAdapter.upsertOne(state, action.payload)
      },
    )

    builder
      .addCase(deleteJobApplication.pending, (state) => {
        state.deleteStatus = 'loading'
        state.deleteError = null
      })
      .addCase(deleteJobApplication.fulfilled, (state, action) => {
        state.deleteStatus = 'succeeded'
        jobApplicationsAdapter.removeOne(state, action.payload)
        state.pagination.totalCount = Math.max(0, state.pagination.totalCount - 1)
      })
      .addCase(deleteJobApplication.rejected, (state, action) => {
        state.deleteStatus = 'failed'
        state.deleteError = (action.payload as string) ?? action.error.message ?? null
      })
  },
})

export const { setFilters, setPage, setPageSize, setStatusFilter, resetMutationState, resetDeleteState } =
  jobApplicationsSlice.actions

export default jobApplicationsSlice.reducer

/**
 * Entity adapter selectors scoped to the job applications slice.
 */
export const jobApplicationsSelectors = jobApplicationsAdapter.getSelectors<RootState>(
  (state) => state.jobApplications,
)

/**
 * Returns the full job applications state slice.
 *
 * @param {RootState} state Redux store state.
 * @returns {JobApplicationsState} Job applications slice.
 */
export const selectJobApplicationsState = (state: RootState) => state.jobApplications
/**
 * Selects the status for list retrieval requests.
 *
 * @param {RootState} state Redux store state.
 * @returns {RequestStatus} Current list request status.
 */
export const selectJobApplicationsListStatus = (state: RootState) =>
  state.jobApplications.listStatus
/**
 * Selects the last error encountered while listing applications.
 *
 * @param {RootState} state Redux store state.
 * @returns {string | null} Error message if one exists.
 */
export const selectJobApplicationsError = (state: RootState) => state.jobApplications.listError
/**
 * Selects the current pagination metadata.
 *
 * @param {RootState} state Redux store state.
 * @returns {PaginationState} Pagination info from the slice.
 */
export const selectJobApplicationsPagination = (state: RootState) =>
  state.jobApplications.pagination
/**
 * Selects the currently applied filters.
 *
 * @param {RootState} state Redux store state.
 * @returns {JobApplicationsState['currentFilters']} Filter configuration.
 */
export const selectJobApplicationsFilters = (state: RootState) =>
  state.jobApplications.currentFilters
/**
 * Selects the mutation request status.
 *
 * @param {RootState} state Redux store state.
 * @returns {RequestStatus} Mutation request status.
 */
export const selectJobApplicationsMutationStatus = (state: RootState) =>
  state.jobApplications.mutationStatus
/**
 * Selects the last mutation error message.
 *
 * @param {RootState} state Redux store state.
 * @returns {string | null} Mutation error message.
 */
export const selectJobApplicationsMutationError = (state: RootState) =>
  state.jobApplications.mutationError
/**
 * Selects the delete request status.
 *
 * @param {RootState} state Redux store state.
 * @returns {RequestStatus} Delete request status.
 */
export const selectJobApplicationsDeleteStatus = (state: RootState) =>
  state.jobApplications.deleteStatus
/**
 * Selects the last delete error message.
 *
 * @param {RootState} state Redux store state.
 * @returns {string | null} Delete error message.
 */
export const selectJobApplicationsDeleteError = (state: RootState) =>
  state.jobApplications.deleteError
