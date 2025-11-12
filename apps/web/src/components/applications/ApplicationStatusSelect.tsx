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

const ApplicationStatusSelect = ({
  id,
  name,
  value,
  onChange,
  disabled = false,
  className,
}: ApplicationStatusSelectProps) => {
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

