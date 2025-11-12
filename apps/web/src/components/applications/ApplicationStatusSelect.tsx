import type { ChangeEvent } from 'react'

import {
  getJobApplicationStatusLabel,
  jobApplicationStatuses,
  type JobApplicationStatus,
} from '@web/types/jobApplications'

type ApplicationStatusSelectProps = {
  id?: string
  name?: string
  value: JobApplicationStatus
  onChange: (status: JobApplicationStatus) => void
  disabled?: boolean
  className?: string
}

/**
 * Displays a select input for job application statuses that normalizes events to domain values.
 *
 * @param {ApplicationStatusSelectProps} props Select props including handlers and disabled state.
 * @returns {JSX.Element} Controlled status dropdown.
 */
const ApplicationStatusSelect = ({
  id,
  name,
  value,
  onChange,
  disabled = false,
  className,
}: ApplicationStatusSelectProps) => {
  /**
   * Normalizes the native change event to emit a typed status value.
   *
   * @param {ChangeEvent<HTMLSelectElement>} event Native change event from the select element.
   */
  const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {
    onChange(event.target.value as JobApplicationStatus)
  }

  return (
    <select
      id={id}
      name={name}
      className={className}
      value={value}
      onChange={handleChange}
      disabled={disabled}
    >
      {jobApplicationStatuses.map((status) => (
        <option key={status} value={status}>
          {getJobApplicationStatusLabel(status)}
        </option>
      ))}
    </select>
  )
}

export default ApplicationStatusSelect

