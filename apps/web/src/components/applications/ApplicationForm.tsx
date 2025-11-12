import { zodResolver } from '@hookform/resolvers/zod'
import { type Resolver, type SubmitHandler, useForm } from 'react-hook-form'

import {
  getJobApplicationStatusLabel,
  jobApplicationFormSchema,
  jobApplicationStatuses,
  type JobApplicationFormValues,
  type JobApplicationStatus,
} from '@web/types/jobApplications'

type ApplicationFormProps = {
  defaultValues?: Partial<JobApplicationFormValues>
  onSubmit: SubmitHandler<JobApplicationFormValues>
  onCancel?: () => void
  isSubmitting?: boolean
  submitLabel?: string
}

const ApplicationForm = ({
  defaultValues,
  onSubmit,
  onCancel,
  isSubmitting = false,
  submitLabel = 'Save application',
}: ApplicationFormProps) => {
  const initialValues: JobApplicationFormValues = {
    companyName: defaultValues?.companyName ?? '',
    position: defaultValues?.position ?? '',
    status: defaultValues?.status ?? 'Applied',
    dateApplied:
      defaultValues?.dateApplied ?? new Date().toISOString().slice(0, 10),
    notes: defaultValues?.notes ?? '',
  }

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<JobApplicationFormValues>({
    resolver: zodResolver(jobApplicationFormSchema) as Resolver<JobApplicationFormValues>,
    defaultValues: initialValues,
  })

  const submit = handleSubmit((values) => onSubmit(values))

  const renderStatusOptions = () =>
    jobApplicationStatuses.map((status: JobApplicationStatus) => (
      <option key={status} value={status}>
        {getJobApplicationStatusLabel(status)}
      </option>
    ))

  return (
    <form className="application-form" onSubmit={submit} noValidate>
      <div className="application-form__grid">
        <label htmlFor="companyName">
          Company Name
          <input
            id="companyName"
            type="text"
            placeholder="Acme Corp"
            autoComplete="organization"
            {...register('companyName')}
          />
          {errors.companyName ? (
            <span className="application-form__error">{errors.companyName.message}</span>
          ) : null}
        </label>

        <label htmlFor="position">
          Position
          <input
            id="position"
            type="text"
            placeholder="Product Manager"
            autoComplete="organization-title"
            {...register('position')}
          />
          {errors.position ? (
            <span className="application-form__error">{errors.position.message}</span>
          ) : null}
        </label>

        <label htmlFor="status">
          Status
          <select id="status" {...register('status')}>
            {renderStatusOptions()}
          </select>
          {errors.status ? (
            <span className="application-form__error">{errors.status.message}</span>
          ) : null}
        </label>

        <label htmlFor="dateApplied">
          Date Applied
          <input id="dateApplied" type="date" {...register('dateApplied')} />
          {errors.dateApplied ? (
            <span className="application-form__error">{errors.dateApplied.message}</span>
          ) : null}
        </label>
      </div>

      <label htmlFor="notes" className="application-form__notes">
        Notes
        <textarea
          id="notes"
          rows={4}
          placeholder="Interview feedback, recruiter details, next steps..."
          {...register('notes')}
        />
        {errors.notes ? (
          <span className="application-form__error">{errors.notes.message}</span>
        ) : null}
      </label>

      <div className="application-form__actions">
        {onCancel ? (
          <button type="button" className="application-form__secondary" onClick={onCancel}>
            Cancel
          </button>
        ) : null}
        <button type="submit" className="application-form__primary" disabled={isSubmitting}>
          {isSubmitting ? 'Saving...' : submitLabel}
        </button>
      </div>
    </form>
  )
}

export default ApplicationForm

