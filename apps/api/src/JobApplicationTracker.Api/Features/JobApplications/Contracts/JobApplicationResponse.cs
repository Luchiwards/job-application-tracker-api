using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

/// <summary>
/// Represents a job application returned by the API.
/// </summary>
public sealed record JobApplicationResponse(
    /// <summary>
    /// Unique identifier of the job application.
    /// </summary>
    int Id,
    /// <summary>
    /// Name of the company the application was submitted to.
    /// </summary>
    string CompanyName,
    /// <summary>
    /// Position or role that was applied for.
    /// </summary>
    string Position,
    /// <summary>
    /// Current status of the job application.
    /// </summary>
    ApplicationStatus Status,
    /// <summary>
    /// Date the application was originally submitted.
    /// </summary>
    DateOnly DateApplied,
    /// <summary>
    /// Optional notes or comments about the application.
    /// </summary>
    string? Notes,
    /// <summary>
    /// Date and time when the application was last updated.
    /// </summary>
    DateTimeOffset LastUpdatedOn);

