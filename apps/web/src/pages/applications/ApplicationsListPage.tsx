import { memo, useCallback, useEffect, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'

import JobApplicationsTable from '@web/components/applications/JobApplicationsTable'
import Alert from '@web/components/shared/Alert'
import PaginationControls from '@web/components/shared/PaginationControls'
import ApplicationDetailsCard from '@web/components/applications/ApplicationDetailsCard'
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

/**
 * Filters the applications list based on status while persisting selection in Redux state.
 *
 * @returns {JSX.Element} Status select wrapped with loading-aware state.
 */
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

/**
 * Displays the list header with filters and primary CTA to add new applications.
 *
 * @param {ApplicationsHeaderProps} props Header callbacks.
 * @returns {JSX.Element} Header content for the applications page.
 */
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
  onSelect: (application: JobApplication) => void
  onPageChange: (page: number) => void
  onPageSizeChange: (pageSize: number) => void
  statusDisabled: boolean
  deleteDisabled: boolean
}

/**
 * Wraps the applications table with loading states, error alerts, and pagination controls.
 *
 * @param {ApplicationsTableSectionProps} props Table handlers and UI flags.
 * @returns {JSX.Element} Applications table section.
 */
const ApplicationsTableSection = memo(
  ({
    onEdit,
    onStatusChange,
    onDelete,
    onSelect,
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
          onSelect={onSelect}
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

/**
 * Coordinates the job applications list view including filters, table interactions, and detail overlay.
 *
 * @returns {JSX.Element} Page content for browsing job applications.
 */
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
  const [selectedApplicationId, setSelectedApplicationId] = useState<number | null>(null)

  // Fetch the first page of applications when the list mounts.
  useEffect(() => {
    dispatch(fetchJobApplications())
  }, [dispatch])

  // Surface success or error feedback when a status mutation completes.
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

  // Refresh list data once deletions complete and surface outcomes.
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

  // Display navigation-driven success messages then clear the history state.
  useEffect(() => {
    if (location.state && typeof location.state === 'object' && 'message' in location.state) {
      const message = String(location.state.message)
      setSuccessMessage(message)
      navigate(location.pathname, { replace: true, state: null })
    }
  }, [location, navigate])

  /**
   * Navigates to the create page for a new job application.
   */
  const handleAddNew = useCallback(() => {
    navigate('/applications/new')
  }, [navigate])

  /**
   * Navigates to the edit page for the selected application.
   *
   * @param {number} id Identifier of the job application.
   */
  const handleEdit = useCallback(
    (id: number) => {
      navigate(`/applications/${id}/edit`)
    },
    [navigate],
  )

  /**
   * Dispatches a status change request for a specific application.
   *
   * @param {number} id Identifier of the application to update.
   * @param {JobApplicationStatus} status New status to persist.
   * @param {JobApplication} application Current application data used to build payload.
   */
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

  /**
   * Dispatches a delete request for a specific application.
   *
   * @param {number} id Identifier of the application to delete.
   */
  const handleDelete = useCallback(
    (id: number) => {
      setDeleteRequested(id)
      dispatch(deleteJobApplication(id))
    },
    [dispatch],
  )

  /**
   * Tracks a selected application to display its details overlay.
   *
   * @param {JobApplication} application Selected job application.
   */
  const handleSelectApplication = useCallback(
    (application: JobApplication) => {
      setSelectedApplicationId(application.id)
    },
    [],
  )

  /**
   * Closes the application detail overlay.
   */
  const handleCloseDetails = useCallback(() => {
    setSelectedApplicationId(null)
  }, [])

  /**
   * Updates the current page in Redux and fetches the next page.
   *
   * @param {number} page Page number to load.
   */
  const handlePageChange = useCallback(
    (page: number) => {
      dispatch(fetchJobApplications({ page }))
    },
    [dispatch],
  )

  /**
   * Updates the page size and refreshes the list from the first page.
   *
   * @param {number} pageSize Page size to request.
   */
  const handlePageSizeChange = useCallback(
    (pageSize: number) => {
      dispatch(fetchJobApplications({ pageSize, page: 1 }))
    },
    [dispatch],
  )

  const isMutationLoading = mutationStatus === 'loading'
  const isDeleteLoading = deleteStatus === 'loading'

  const selectedApplication = useAppSelector((state) =>
    selectedApplicationId !== null ? jobApplicationsSelectors.selectById(state, selectedApplicationId) ?? null : null,
  )

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

  // Clear the selected application if it disappears from the store (e.g., deleted).
  useEffect(() => {
    if (selectedApplicationId !== null && !selectedApplication) {
      setSelectedApplicationId(null)
    }
  }, [selectedApplicationId, selectedApplication])

  // Close the overlay when users press Escape while details are open.
  useEffect(() => {
    if (!selectedApplication) {
      return
    }

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        setSelectedApplicationId(null)
      }
    }

    window.addEventListener('keydown', handleKeyDown)
    return () => window.removeEventListener('keydown', handleKeyDown)
  }, [selectedApplication])

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
        onSelect={handleSelectApplication}
      />

      {selectedApplication ? (
        <div
          className="applications-overlay"
          role="dialog"
          aria-modal="true"
          aria-labelledby="application-details-title"
          onClick={handleCloseDetails}
        >
          <div
            className="applications-overlay__content"
            onClick={(event) => event.stopPropagation()}
          >
            <ApplicationDetailsCard application={selectedApplication} onClose={handleCloseDetails} />
          </div>
        </div>
      ) : null}
    </section>
  )
}

export default ApplicationsListPage

