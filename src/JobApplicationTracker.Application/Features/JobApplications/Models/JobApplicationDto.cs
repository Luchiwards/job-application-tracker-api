using System;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Models;

public sealed record JobApplicationDto(
    Guid Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes,
    DateTimeOffset LastUpdatedOn)
{
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

