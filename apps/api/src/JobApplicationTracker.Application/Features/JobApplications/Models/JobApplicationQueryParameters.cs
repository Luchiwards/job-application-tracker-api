using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Models;

public sealed record JobApplicationQueryParameters(
    int Page,
    int PageSize,
    ApplicationStatus? Status,
    string? SearchTerm,
    DateOnly? AppliedFrom,
    DateOnly? AppliedTo);

