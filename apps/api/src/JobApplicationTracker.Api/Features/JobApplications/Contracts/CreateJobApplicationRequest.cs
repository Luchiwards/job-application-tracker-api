using System;
using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Api.Features.JobApplications.Contracts;

/// <summary>
/// Represents the payload required to create a new job application.
/// </summary>
public sealed class CreateJobApplicationRequest
{
    /// <summary>
    /// Name of the company the application was submitted to.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Position or role that was applied for.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the job application.
    /// </summary>
    [Required]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    /// <summary>
    /// Date the application was submitted.
    /// </summary>
    [Required]
    public DateOnly DateApplied { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Optional notes or comments about the application. Limited to 2000 characters.
    /// </summary>
    [StringLength(2000)]
    public string? Notes { get; set; }
}

