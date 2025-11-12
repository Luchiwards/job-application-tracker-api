using System;
using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

/// <summary>
/// Query parameters used to filter and paginate job application results.
/// </summary>
public sealed class GetJobApplicationsRequest
{
    /// <summary>
    /// Page number to retrieve. Must be greater than or equal to 1.
    /// </summary>
    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// Number of records to include in each page. Must be between 1 and 100.
    /// </summary>
    [FromQuery(Name = "pageSize")]
    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Filter results to job applications in the provided status.
    /// </summary>
    [FromQuery(Name = "status")]
    public ApplicationStatus? Status { get; init; }

    /// <summary>
    /// Filter results to job applications whose company name or position contains the provided text.
    /// </summary>
    [FromQuery(Name = "searchTerm")]
    [StringLength(200)]
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Filter results to job applications submitted on or after the provided date.
    /// </summary>
    [FromQuery(Name = "appliedFrom")]
    public DateOnly? AppliedFrom { get; init; }

    /// <summary>
    /// Filter results to job applications submitted on or before the provided date.
    /// </summary>
    [FromQuery(Name = "appliedTo")]
    public DateOnly? AppliedTo { get; init; }
}

