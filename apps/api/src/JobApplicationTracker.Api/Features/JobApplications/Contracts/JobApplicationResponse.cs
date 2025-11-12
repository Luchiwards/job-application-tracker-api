using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

/// <summary>
/// Represents a job application returned by the API.
/// </summary>
/// <param name="Id">Unique identifier of the job application.</param>
/// <param name="CompanyName">Name of the company the application was submitted to.</param>
/// <param name="Position">Position or role that was applied for.</param>
/// <param name="Status">Current status of the job application.</param>
/// <param name="DateApplied">Date the application was originally submitted.</param>
/// <param name="Notes">Optional notes or comments about the application.</param>
/// <param name="LastUpdatedOn">Date and time when the application was last updated.</param>
public sealed record JobApplicationResponse(
    int Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes,
    DateTimeOffset LastUpdatedOn);

