import { useEffect, useMemo } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

import ApplicationForm from '@web/components/applications/ApplicationForm'
import Alert from '@web/components/shared/Alert'
import { useAppDispatch, useAppSelector } from '@web/store/hooks'
import {
  fetchJobApplicationById,
  jobApplicationsSelectors,
  resetMutationState,
  selectJobApplicationsMutationError,
  selectJobApplicationsMutationStatus,
  updateJobApplication,
} from '@web/store/jobApplicationsSlice'
import {
  mapApplicationToFormValues,
  mapFormValuesToPayload,
  type JobApplicationFormValues,
} from '@web/types/jobApplications'

/**
 * Retrieves an application for editing and coordinates update mutations.
 *
 * @returns {JSX.Element} Page layout for editing a job application.
 */
const ApplicationEditPage = () => {
  const { id } = useParams()
  const applicationId = Number(id)
  const dispatch = useAppDispatch()
  const navigate = useNavigate()

  const application = useAppSelector((state) => {
    if (Number.isNaN(applicationId)) {
      return null
    }
    return jobApplicationsSelectors.selectById(state, applicationId) ?? null
  })

  const mutationStatus = useAppSelector(selectJobApplicationsMutationStatus)
  const mutationError = useAppSelector(selectJobApplicationsMutationError)

  const isInvalidId = Number.isNaN(applicationId)
  const isLoading = !application

  // Fetch the application on mount and reset mutation state on unmount.
  useEffect(() => {
    if (isInvalidId) {
      return
    }

    if (!application) {
      dispatch(fetchJobApplicationById(applicationId))
    }

    return () => {
      dispatch(resetMutationState())
    }
  }, [dispatch, applicationId, application, isInvalidId])

  /**
   * Prepares default form values derived from the application entity.
   *
   * @returns {JobApplicationFormValues | undefined} Form values once the application is loaded.
   */
  const defaultValues = useMemo(() => {
    if (!application) {
      return undefined
    }

    return mapApplicationToFormValues(application)
  }, [application])

  /**
   * Persists changes to the application and redirects to the list view when done.
   *
   * @param {JobApplicationFormValues} values Form values submitted by the user.
   * @returns {Promise<void>} Promise resolving after navigation.
   */
  const handleSubmit = async (values: JobApplicationFormValues) => {
    if (isInvalidId) {
      return
    }

    const result = await dispatch(
      updateJobApplication({ id: applicationId, payload: mapFormValuesToPayload(values) }),
    )

    if (updateJobApplication.fulfilled.match(result)) {
      navigate('/applications', { state: { message: 'Application updated successfully.' } })
    }
  }

  /**
   * Returns the user to the applications list without saving changes.
   */
  const handleCancel = () => {
    navigate('/applications')
  }

  if (isInvalidId) {
    return (
      <section className="application-form-container">
        <h1>Invalid application</h1>
        <p>The requested application identifier is not valid.</p>
      </section>
    )
  }

  if (isLoading || !application || !defaultValues) {
    return (
      <section className="application-form-container" role="status" aria-live="polite">
        <p>Loading application details...</p>
      </section>
    )
  }

  return (
    <section className="application-form-container">
      <h1>Edit job application</h1>
      <p className="application-form-container__subtitle">
        Update the details to keep your pipeline accurate.
      </p>
      {mutationError ? <Alert variant="error">{mutationError}</Alert> : null}
      <ApplicationForm
        defaultValues={defaultValues}
        onSubmit={handleSubmit}
        onCancel={handleCancel}
        isSubmitting={mutationStatus === 'loading'}
        submitLabel="Save changes"
      />
      {application ? (
        <div className="application-form-container__meta">
          Last updated:{' '}
          {new Date(application.lastUpdatedOn).toLocaleString(undefined, {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
          })}
        </div>
      ) : null}
    </section>
  )
}

export default ApplicationEditPage

