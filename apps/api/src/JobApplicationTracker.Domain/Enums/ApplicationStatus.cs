namespace JobApplicationTracker.Domain.Enums;

/// <summary>
/// Represents the lifecycle stages of a job application.
/// </summary>
public enum ApplicationStatus
{
    /// <summary>
    /// Application has been submitted and is awaiting review.
    /// </summary>
    Applied = 1,

    /// <summary>
    /// Application has passed initial screening and is shortlisted.
    /// </summary>
    Shortlisted = 2,

    /// <summary>
    /// Candidate is scheduled for or participating in interviews.
    /// </summary>
    Interview = 3,

    /// <summary>
    /// Post-interview evaluation is in progress and a decision is pending.
    /// </summary>
    InProcess = 4,

    /// <summary>
    /// Candidate has received an offer from the company.
    /// </summary>
    Offer = 5,

    /// <summary>
    /// Candidate declined the offer or the application was rejected.
    /// </summary>
    Declined = 6,

    /// <summary>
    /// Hiring process has closed without filling the position.
    /// </summary>
    PositionClosed = 7
}

