using System;
using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

public sealed class GetJobApplicationsRequest
{
    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "pageSize")]
    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    [FromQuery(Name = "status")]
    public ApplicationStatus? Status { get; init; }

    [FromQuery(Name = "searchTerm")]
    [StringLength(200)]
    public string? SearchTerm { get; init; }

    [FromQuery(Name = "appliedFrom")]
    public DateOnly? AppliedFrom { get; init; }

    [FromQuery(Name = "appliedTo")]
    public DateOnly? AppliedTo { get; init; }
}

