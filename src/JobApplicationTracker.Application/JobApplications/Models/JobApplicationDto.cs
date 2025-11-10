using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.JobApplications.Models;

public sealed record JobApplicationDto(
    Guid Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes,
    DateTimeOffset LastUpdatedOn);

