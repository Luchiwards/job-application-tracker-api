import type { JobApplication } from '@web/types/jobApplications'
import { getJobApplicationStatusLabel } from '@web/types/jobApplications'
import { formatDate, formatDateTime } from '@web/utils/date'

type ApplicationDetailsCardProps = {
  application: JobApplication
  onClose: () => void
}

/**
 * Renders a modal-style card with the selected job application metadata and notes.
 *
 * @param {ApplicationDetailsCardProps} props Component props containing the application and close handler.
 * @returns {JSX.Element} Detail view for a single job application.
 */
const ApplicationDetailsCard = ({ application, onClose }: ApplicationDetailsCardProps) => {
  return (
    <article className="application-details" aria-labelledby="application-details-title">
      <header className="application-details__header">
        <div className="application-details__title">
          <h2 id="application-details-title">{application.companyName}</h2>
          <p className="application-details__subtitle">{application.position}</p>
        </div>
        <button
          type="button"
          className="application-details__close"
          onClick={onClose}
          aria-label="Close application details"
        >
          <span aria-hidden="true">&times;</span>
        </button>
      </header>
      <div className="application-details__body">
        <dl className="application-details__meta">
          <div>
            <dt>Status</dt>
            <dd>{getJobApplicationStatusLabel(application.status)}</dd>
          </div>
          <div>
            <dt>Date Applied</dt>
            <dd>{formatDate(application.dateApplied)}</dd>
          </div>
          <div>
            <dt>Last Updated</dt>
            <dd>{formatDateTime(application.lastUpdatedOn)}</dd>
          </div>
        </dl>
        <section className="application-details__notes">
          <h3>Notes</h3>
          <p>{application.notes ? application.notes : 'No notes provided.'}</p>
        </section>
      </div>
    </article>
  )
}

export default ApplicationDetailsCard


