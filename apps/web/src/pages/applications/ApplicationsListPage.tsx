import { memo, useCallback, useEffect, useState } from 'react'
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

const StatusFilter = memo(() => {
  const dispatch = useAppDispatch()
  const listStatus = useAppSelector(selectJobApplicationsListStatus)
  const filters = useAppSelector(selectJobApplicationsFilters)

  const statusFilterValue = filters.status ?? 'all'
  const isLoading = listStatus === 'loading'

  const handleStatusFilterChange = useCallback(
    (statusValue: string) => {
      const nextStatus =
        statusValue === 'all' ? undefined : (statusValue as JobApplicationStatus)
      dispatch(fetchJobApplications({ status: nextStatus, page: 1 }))
    },
    [dispatch],
  )

  return (
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
          {jobApplicationStatuses.map((status) => (
            <option key={status} value={status}>
              {getJobApplicationStatusLabel(status)}
            </option>
          ))}
        </select>
      </label>
    </div>
  )
})

StatusFilter.displayName = 'StatusFilter'

type ApplicationsHeaderProps = {
  onAddNew: () => void
}

const ApplicationsHeader = memo(({ onAddNew }: ApplicationsHeaderProps) => (
  <header className="applications__header">
    <StatusFilter />
    <button type="button" className="applications__primary" onClick={onAddNew}>
      Add new application
    </button>
  </header>
))

ApplicationsHeader.displayName = 'ApplicationsHeader'

type ApplicationsTableSectionProps = {
  onEdit: (id: number) => void
  onStatusChange: (id: number, status: JobApplicationStatus, application: JobApplication) => void
  onDelete: (id: number) => void
  onPageChange: (page: number) => void
  onPageSizeChange: (pageSize: number) => void
  statusDisabled: boolean
  deleteDisabled: boolean
}

const ApplicationsTableSection = memo(
  ({
    onEdit,
    onStatusChange,
    onDelete,
    onPageChange,
    onPageSizeChange,
    statusDisabled,
    deleteDisabled,
  }: ApplicationsTableSectionProps) => {
    const applications = useAppSelector(jobApplicationsSelectors.selectAll)
    const listStatus = useAppSelector(selectJobApplicationsListStatus)
    const listError = useAppSelector(selectJobApplicationsError)
    const pagination = useAppSelector(selectJobApplicationsPagination)

    const isLoading = listStatus === 'loading'

    return (
      <>
        {listError ? <Alert variant="error">{listError}</Alert> : null}
        <JobApplicationsTable
          applications={applications}
          onEdit={onEdit}
          onStatusChange={onStatusChange}
          onDelete={onDelete}
          isLoading={isLoading}
          statusDisabled={statusDisabled}
          deleteDisabled={deleteDisabled}
        />
        <PaginationControls
          page={pagination.page}
          pageSize={pagination.pageSize}
          totalPages={pagination.totalPages}
          totalCount={pagination.totalCount}
          onPageChange={onPageChange}
          onPageSizeChange={onPageSizeChange}
          isLoading={isLoading}
        />
      </>
    )
  },
)

ApplicationsTableSection.displayName = 'ApplicationsTableSection'

const ApplicationsListPage = () => {
  const dispatch = useAppDispatch()
  const navigate = useNavigate()
  const location = useLocation()

  const mutationStatus = useAppSelector(selectJobApplicationsMutationStatus)
  const mutationError = useAppSelector(selectJobApplicationsMutationError)
  const deleteStatus = useAppSelector(selectJobApplicationsDeleteStatus)
  const deleteError = useAppSelector(selectJobApplicationsDeleteError)

  const [statusUpdateRequested, setStatusUpdateRequested] = useState(false)
  const [deleteRequested, setDeleteRequested] = useState<number | null>(null)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)

  useEffect(() => {
    dispatch(fetchJobApplications())
  }, [dispatch])

  useEffect(() => {
    if (!statusUpdateRequested) {
      return
    }

    if (mutationStatus === 'succeeded') {
      setSuccessMessage('Application status updated.')
      setStatusUpdateRequested(false)
      dispatch(resetMutationState())
      return
    }

    if (mutationStatus === 'failed') {
      setStatusUpdateRequested(false)
    }
  }, [dispatch, mutationStatus, statusUpdateRequested])

  useEffect(() => {
    if (deleteRequested === null) {
      return
    }

    if (deleteStatus === 'succeeded') {
      setSuccessMessage('Application deleted.')
      setDeleteRequested(null)
      dispatch(resetDeleteState())
      dispatch(fetchJobApplications())
      return
    }

    if (deleteStatus === 'failed') {
      setDeleteRequested(null)
    }
  }, [deleteRequested, deleteStatus, dispatch])

  useEffect(() => {
    if (location.state && typeof location.state === 'object' && 'message' in location.state) {
      const message = String(location.state.message)
      setSuccessMessage(message)
      navigate(location.pathname, { replace: true, state: null })
    }
  }, [location, navigate])

  const handleAddNew = useCallback(() => {
    navigate('/applications/new')
  }, [navigate])

  const handleEdit = useCallback(
    (id: number) => {
      navigate(`/applications/${id}/edit`)
    },
    [navigate],
  )

  const handleStatusChange = useCallback(
    (id: number, status: JobApplicationStatus, application: JobApplication) => {
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
    },
    [dispatch],
  )

  const handleDelete = useCallback(
    (id: number) => {
      setDeleteRequested(id)
      dispatch(deleteJobApplication(id))
    },
    [dispatch],
  )

  const handlePageChange = useCallback(
    (page: number) => {
      dispatch(fetchJobApplications({ page }))
    },
    [dispatch],
  )

  const handlePageSizeChange = useCallback(
    (pageSize: number) => {
      dispatch(fetchJobApplications({ pageSize, page: 1 }))
    },
    [dispatch],
  )

  const isMutationLoading = mutationStatus === 'loading'
  const isDeleteLoading = deleteStatus === 'loading'

  const alerts: Array<{
    key: string
    variant: 'error' | 'success'
    message: string
    onClose: () => void
  }> = []

  if (mutationError) {
    alerts.push({
      key: 'mutationError',
      variant: 'error',
      message: mutationError,
      onClose: () => {
        dispatch(resetMutationState())
      },
    })
  }

  if (deleteError) {
    alerts.push({
      key: 'deleteError',
      variant: 'error',
      message: deleteError,
      onClose: () => {
        dispatch(resetDeleteState())
      },
    })
  }

  if (successMessage) {
    alerts.push({
      key: 'successMessage',
      variant: 'success',
      message: successMessage,
      onClose: () => {
        setSuccessMessage(null)
      },
    })
  }

  return (
    <section className="applications">
      <ApplicationsHeader onAddNew={handleAddNew} />

      {alerts.map(({ key, variant, message, onClose }) => (
        <Alert key={key} variant={variant} dismissible onClose={onClose}>
          {message}
        </Alert>
      ))}

      <ApplicationsTableSection
        onEdit={handleEdit}
        onStatusChange={handleStatusChange}
        onDelete={handleDelete}
        onPageChange={handlePageChange}
        onPageSizeChange={handlePageSizeChange}
        statusDisabled={isMutationLoading}
        deleteDisabled={isDeleteLoading}
      />
    </section>
  )
}

export default ApplicationsListPage

