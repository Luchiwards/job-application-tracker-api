import { useEffect, useMemo, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'

import JobApplicationsTable from '@web/components/applications/JobApplicationsTable'
import Alert from '@web/components/shared/Alert'
import PaginationControls from '@web/components/shared/PaginationControls'
import { useAppDispatch, useAppSelector } from '@web/store/hooks'
import {
  changeJobApplicationStatus,
  deleteJobApplication,
  fetchJobApplications,
  jobApplicationsSelectors,
  resetDeleteState,
  resetMutationState,
  selectJobApplicationsDeleteError,
  selectJobApplicationsDeleteStatus,
  selectJobApplicationsError,
  selectJobApplicationsFilters,
  selectJobApplicationsListStatus,
  selectJobApplicationsMutationError,
  selectJobApplicationsMutationStatus,
  selectJobApplicationsPagination,
} from '@web/store/jobApplicationsSlice'
import {
  getJobApplicationStatusLabel,
  jobApplicationStatuses,
  type JobApplication,
  type JobApplicationStatus,
} from '@web/types/jobApplications'

const ApplicationsListPage = () => {
  const dispatch = useAppDispatch()
  const navigate = useNavigate()
  const location = useLocation()

  const applications = useAppSelector(jobApplicationsSelectors.selectAll)
  const listStatus = useAppSelector(selectJobApplicationsListStatus)
  const listError = useAppSelector(selectJobApplicationsError)
  const pagination = useAppSelector(selectJobApplicationsPagination)
  const filters = useAppSelector(selectJobApplicationsFilters)
  const mutationStatus = useAppSelector(selectJobApplicationsMutationStatus)
  const mutationError = useAppSelector(selectJobApplicationsMutationError)
  const deleteStatus = useAppSelector(selectJobApplicationsDeleteStatus)
  const deleteError = useAppSelector(selectJobApplicationsDeleteError)

  const [statusUpdateRequested, setStatusUpdateRequested] = useState(false)
  const [deleteRequested, setDeleteRequested] = useState<number | null>(null)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)

  useEffect(() => {
    if (listStatus === 'idle') {
      dispatch(fetchJobApplications(undefined))
    }
  }, [dispatch, listStatus])

  const isLoading = listStatus === 'loading'
  const isMutationLoading = mutationStatus === 'loading'

  const uniqueStatuses = useMemo(() => jobApplicationStatuses, [])

  useEffect(() => {
    if (statusUpdateRequested) {
      if (mutationStatus === 'succeeded') {
        setSuccessMessage('Application status updated.')
        setStatusUpdateRequested(false)
        dispatch(resetMutationState())
      }

      if (mutationStatus === 'failed') {
        setStatusUpdateRequested(false)
      }
    }
  }, [dispatch, mutationStatus, statusUpdateRequested])

  useEffect(() => {
    if (deleteRequested !== null) {
      if (deleteStatus === 'succeeded') {
        setSuccessMessage('Application deleted.')
        setDeleteRequested(null)
        dispatch(resetDeleteState())
        dispatch(fetchJobApplications(filters))
      }

      if (deleteStatus === 'failed') {
        setDeleteRequested(null)
      }
    }
  }, [deleteRequested, deleteStatus, dispatch, filters])

  useEffect(() => {
    if (location.state && typeof location.state === 'object' && 'message' in location.state) {
      const message = String(location.state.message)
      setSuccessMessage(message)
      navigate(location.pathname, { replace: true, state: null })
    }
  }, [location, navigate])

  const handleAddNew = () => {
    navigate('/applications/new')
  }

  const handlePageChange = (page: number) => {
    dispatch(fetchJobApplications({ page }))
  }

  const handlePageSizeChange = (pageSize: number) => {
    dispatch(fetchJobApplications({ pageSize, page: 1 }))
  }

  const handleStatusFilterChange = (statusValue: string) => {
    const nextStatus =
      statusValue === 'all' ? undefined : (statusValue as JobApplicationStatus)
    dispatch(fetchJobApplications({ status: nextStatus, page: 1 }))
  }

  const handleStatusChange = (
    id: number,
    status: JobApplicationStatus,
    application: JobApplication,
  ) => {
    setStatusUpdateRequested(true)
    dispatch(
      changeJobApplicationStatus({
        id,
        payload: {
          companyName: application.companyName,
          position: application.position,
          status,
          dateApplied: application.dateApplied,
          notes: application.notes ?? null,
        },
      }),
    )
  }

  const statusFilterValue = filters.status ?? 'all'

  return (
    <section className="applications">
      <header className="applications__header">
        <div className="applications__header-content">
          <h1>Job Applications</h1>
          <p className="applications__subtitle">
            Track the status of every opportunity and update progress in real time.
          </p>
        </div>
        <button type="button" className="applications__primary" onClick={handleAddNew}>
          Add new application
        </button>
      </header>

      <div className="applications__filters">
        <label htmlFor="statusFilter">
          Status
          <select
            id="statusFilter"
            value={statusFilterValue}
            onChange={(event) => handleStatusFilterChange(event.target.value)}
            disabled={isLoading}
          >
            <option value="all">All statuses</option>
            {uniqueStatuses.map((status) => (
              <option key={status} value={status}>
                {getJobApplicationStatusLabel(status)}
              </option>
            ))}
          </select>
        </label>
      </div>

      {listError ? <Alert variant="error">{listError}</Alert> : null}
      {mutationError ? (
        <Alert
          variant="error"
          dismissible
          onClose={() => {
            dispatch(resetMutationState())
          }}
        >
          {mutationError}
        </Alert>
      ) : null}
      {deleteError ? (
        <Alert
          variant="error"
          dismissible
          onClose={() => {
            dispatch(resetDeleteState())
          }}
        >
          {deleteError}
        </Alert>
      ) : null}
      {successMessage ? (
        <Alert
          variant="success"
          dismissible
          onClose={() => {
            setSuccessMessage(null)
          }}
        >
          {successMessage}
        </Alert>
      ) : null}

      <JobApplicationsTable
        applications={applications}
        onEdit={(id) => navigate(`/applications/${id}/edit`)}
        onStatusChange={handleStatusChange}
        onDelete={(id) => {
          setDeleteRequested(id)
          dispatch(deleteJobApplication(id))
        }}
        isLoading={isLoading}
        statusDisabled={isMutationLoading}
        deleteDisabled={deleteStatus === 'loading'}
      />

      <PaginationControls
        page={pagination.page}
        pageSize={pagination.pageSize}
        totalPages={pagination.totalPages}
        totalCount={pagination.totalCount}
        onPageChange={handlePageChange}
        onPageSizeChange={handlePageSizeChange}
        isLoading={isLoading}
      />
    </section>
  )
}

export default ApplicationsListPage

