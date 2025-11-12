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
  emptyMessage?: string
}

const JobApplicationsTable = ({
  applications,
  onEdit,
  onStatusChange,
  onDelete,
  isLoading = false,
  statusDisabled = false,
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
                {new Date(application.dateApplied).toLocaleDateString(undefined, {
                  year: 'numeric',
                  month: 'short',
                  day: 'numeric',
                })}
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
                  >
                    Delete
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

