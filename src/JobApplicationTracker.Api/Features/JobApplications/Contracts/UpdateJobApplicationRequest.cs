using System;
using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

public sealed class UpdateJobApplicationRequest
{
    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Position { get; set; } = string.Empty;

    [Required]
    public ApplicationStatus Status { get; set; }

    [Required]
    public DateOnly DateApplied { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }
}

