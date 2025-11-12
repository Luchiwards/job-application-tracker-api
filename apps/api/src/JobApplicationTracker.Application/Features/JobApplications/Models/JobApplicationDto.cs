using System;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Models;

/// <summary>
/// Represents a job application within the application layer.
/// </summary>
/// <param name="Id">Unique identifier of the job application.</param>
/// <param name="CompanyName">Name of the company the application targets.</param>
/// <param name="Position">Position or role applied for.</param>
/// <param name="Status">Current status of the application.</param>
/// <param name="DateApplied">Date the application was submitted.</param>
/// <param name="Notes">Optional notes associated with the application.</param>
/// <param name="LastUpdatedOn">Timestamp of the most recent update.</param>
public sealed record JobApplicationDto(
    int Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes,
    DateTimeOffset LastUpdatedOn)
{
    /// <summary>
    /// Creates a <see cref="JobApplicationDto"/> from a domain entity.
    /// </summary>
    /// <param name="entity">The domain entity to map.</param>
    /// <returns>A DTO representing the supplied domain entity.</returns>
    public static JobApplicationDto FromEntity(JobApplication entity) =>
        new(
            entity.Id,
            entity.CompanyName,
            entity.Position,
            entity.Status,
            entity.DateApplied,
            entity.Notes,
            entity.LastUpdatedOn);
}

