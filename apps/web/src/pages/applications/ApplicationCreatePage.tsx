import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'

import ApplicationForm from '@web/components/applications/ApplicationForm'
import Alert from '@web/components/shared/Alert'
import { useAppDispatch, useAppSelector } from '@web/store/hooks'
import {
  createJobApplication,
  resetMutationState,
  selectJobApplicationsMutationError,
  selectJobApplicationsMutationStatus,
} from '@web/store/jobApplicationsSlice'
import { mapFormValuesToPayload, type JobApplicationFormValues } from '@web/types/jobApplications'

/**
 * Provides the create job application flow and coordinates API interactions.
 *
 * @returns {JSX.Element} Page layout for creating a job application.
 */
const ApplicationCreatePage = () => {
  const dispatch = useAppDispatch()
  const navigate = useNavigate()

  const mutationStatus = useAppSelector(selectJobApplicationsMutationStatus)
  const mutationError = useAppSelector(selectJobApplicationsMutationError)

  // Reset mutation state when leaving the create view to avoid stale errors.
  useEffect(
    () => () => {
      dispatch(resetMutationState())
    },
    [dispatch],
  )

  /**
   * Persists a new job application and redirects to the list on success.
   *
   * @param {JobApplicationFormValues} values Form values submitted by the user.
   * @returns {Promise<void>} Promise resolving when navigation completes.
   */
  const handleSubmit = async (values: JobApplicationFormValues) => {
    const result = await dispatch(createJobApplication(mapFormValuesToPayload(values)))

    if (createJobApplication.fulfilled.match(result)) {
      navigate('/applications', { state: { message: 'Application created successfully.' } })
    }
  }

  /**
   * Returns the user to the applications list without creating a record.
   */
  const handleCancel = () => {
    navigate('/applications')
  }

  return (
    <section className="application-form-container">
      <h1>Create job application</h1>
      <p className="application-form-container__subtitle">
        Capture essential details so you can manage follow-ups and interviews effectively.
      </p>
      {mutationError ? <Alert variant="error">{mutationError}</Alert> : null}
      <ApplicationForm
        onSubmit={handleSubmit}
        onCancel={handleCancel}
        isSubmitting={mutationStatus === 'loading'}
        submitLabel="Create application"
      />
    </section>
  )
}

export default ApplicationCreatePage

