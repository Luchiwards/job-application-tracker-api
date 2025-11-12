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

const ApplicationCreatePage = () => {
  const dispatch = useAppDispatch()
  const navigate = useNavigate()

  const mutationStatus = useAppSelector(selectJobApplicationsMutationStatus)
  const mutationError = useAppSelector(selectJobApplicationsMutationError)

  useEffect(
    () => () => {
      dispatch(resetMutationState())
    },
    [dispatch],
  )

  const handleSubmit = async (values: JobApplicationFormValues) => {
    const result = await dispatch(createJobApplication(mapFormValuesToPayload(values)))

    if (createJobApplication.fulfilled.match(result)) {
      navigate('/applications', { state: { message: 'Application created successfully.' } })
    }
  }

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

