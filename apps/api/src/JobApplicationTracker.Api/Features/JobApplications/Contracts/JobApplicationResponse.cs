using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

public sealed record JobApplicationResponse(
    int Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes,
    DateTimeOffset LastUpdatedOn);

