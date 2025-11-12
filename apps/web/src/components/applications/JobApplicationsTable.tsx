import type { JobApplication, JobApplicationStatus } from '@web/types/jobApplications'
import { getJobApplicationStatusLabel } from '@web/types/jobApplications'

import ApplicationStatusSelect from './ApplicationStatusSelect'

type JobApplicationsTableProps = {
  applications: JobApplication[]
  onEdit: (id: number) => void
  onStatusChange: (id: number, status: JobApplicationStatus, application: JobApplication) => void
  onDelete?: (id: number) => void
  isLoading?: boolean
  statusDisabled?: boolean
  deleteDisabled?: boolean
  emptyMessage?: string
}

const formatDate = (value: string): string => {
  const parsed = new Date(value)
  if (Number.isNaN(parsed.valueOf())) {
    return value
  }

  return parsed.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })
}

const formatDateTime = (value: string): string => {
  const parsed = new Date(value)
  if (Number.isNaN(parsed.valueOf())) {
    return value
  }

  return parsed.toLocaleString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

const JobApplicationsTable = ({
  applications,
  onEdit,
  onStatusChange,
  onDelete,
  isLoading = false,
  statusDisabled = false,
  deleteDisabled = false,
  emptyMessage = 'No job applications found.',
}: JobApplicationsTableProps) => {
  if (isLoading) {
    return (
      <div className="applications-table__placeholder" role="status" aria-live="polite">
        Loading applications...
      </div>
    )
  }

  if (applications.length === 0) {
    return <p className="applications-table__empty">{emptyMessage}</p>
  }

  return (
    <div className="applications-table__container">
      <table className="applications-table">
        <thead>
          <tr>
            <th scope="col">Company</th>
            <th scope="col">Position</th>
            <th scope="col">Status</th>
            <th scope="col">Date Applied</th>
            <th scope="col">Last Updated</th>
            <th scope="col" className="applications-table__actions-header">
              Actions
            </th>
          </tr>
        </thead>
        <tbody>
          {applications.map((application) => (
            <tr key={application.id}>
              <td data-label="Company">{application.companyName}</td>
              <td data-label="Position">{application.position}</td>
              <td data-label="Status">
                <ApplicationStatusSelect
                  value={application.status}
                  onChange={(status) => onStatusChange(application.id, status, application)}
                  disabled={statusDisabled}
                />
              </td>
              <td data-label="Date Applied">
                {formatDate(application.dateApplied)}
              </td>
              <td data-label="Last Updated">
                {formatDateTime(application.lastUpdatedOn)}
              </td>
              <td className="applications-table__actions" data-label="Actions">
                <button
                  type="button"
                  className="applications-table__edit"
                  onClick={() => onEdit(application.id)}
                >
                  Edit
                </button>
                {onDelete ? (
                  <button
                    type="button"
                    className="applications-table__delete"
                    onClick={() => onDelete(application.id)}
                    aria-label={`Delete ${application.companyName} application`}
                    disabled={deleteDisabled}
                  >
                    <svg
                      className="applications-table__delete-icon"
                      width="16"
                      height="16"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                      aria-hidden="true"
                      focusable="false"
                    >
                      <path
                        d="M9 3V4H4.5C4.22386 4 4 4.22386 4 4.5V5.5C4 5.77614 4.22386 6 4.5 6H19.5C19.7761 6 20 5.77614 20 5.5V4.5C20 4.22386 19.7761 4 19.5 4H15V3C15 2.44772 14.5523 2 14 2H10C9.44772 2 9 2.44772 9 3ZM6 20C6 21.1046 6.89543 22 8 22H16C17.1046 22 18 21.1046 18 20V8H6V20Z"
                        fill="currentColor"
                      />
                    </svg>
                  </button>
                ) : null}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="applications-table__legend">
        Status legend:{' '}
        {applications
          .map((application) => getJobApplicationStatusLabel(application.status))
          .filter((value, index, self) => self.indexOf(value) === index)
          .join(', ')}
      </div>
    </div>
  )
}

export default JobApplicationsTable

