using System;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Models;

/// <summary>
/// Encapsulates filtering and pagination criteria for job application searches.
/// </summary>
/// <param name="Page">Page number to retrieve.</param>
/// <param name="PageSize">Number of results per page.</param>
/// <param name="Status">Optional status filter.</param>
/// <param name="SearchTerm">Optional search term applied to company or position.</param>
/// <param name="AppliedFrom">Lower bound for application submission date.</param>
/// <param name="AppliedTo">Upper bound for application submission date.</param>
public sealed record JobApplicationQueryParameters(
    int Page,
    int PageSize,
    ApplicationStatus? Status,
    string? SearchTerm,
    DateOnly? AppliedFrom,
    DateOnly? AppliedTo);

