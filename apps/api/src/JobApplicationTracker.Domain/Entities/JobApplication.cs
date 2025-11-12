using System;

using JobApplicationTracker.Domain.Common;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Domain.Entities;

/// <summary>
/// Represents a job application aggregate root.
/// </summary>
public sealed class JobApplication : Entity
{
    private JobApplication(
        int id,
        string companyName,
        string position,
        ApplicationStatus status,
        DateOnly dateApplied,
        string? notes) : base(id)
    {
        CompanyName = companyName;
        Position = position;
        Status = status;
        DateApplied = dateApplied;
        Notes = notes;
        LastUpdatedOn = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Gets the company name associated with the application.
    /// </summary>
    public string CompanyName { get; private set; } = null!;

    /// <summary>
    /// Gets the position or role for which the application was submitted.
    /// </summary>
    public string Position { get; private set; } = null!;

    /// <summary>
    /// Gets the current status of the application.
    /// </summary>
    public ApplicationStatus Status { get; private set; }

    /// <summary>
    /// Gets the date the application was submitted.
    /// </summary>
    public DateOnly DateApplied { get; private set; }

    /// <summary>
    /// Gets optional notes about the application.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets the timestamp of the most recent change to the application.
    /// </summary>
    public DateTimeOffset LastUpdatedOn { get; private set; }

    /// <summary>
    /// Creates a new <see cref="JobApplication"/> instance.
    /// </summary>
    /// <param name="companyName">Company name associated with the application.</param>
    /// <param name="position">Position or role applied for.</param>
    /// <param name="status">Initial status of the application.</param>
    /// <param name="dateApplied">Date the application was submitted.</param>
    /// <param name="notes">Optional notes about the application.</param>
    /// <param name="id">Optional identifier used when rehydrating from persistence.</param>
    /// <returns>A new job application.</returns>
    public static JobApplication Create(
        string companyName,
        string position,
        ApplicationStatus status,
        DateOnly dateApplied,
        string? notes = null,
        int? id = null)
    {
        return new JobApplication(
            id ?? 0,
            Guard.AgainstNullOrWhiteSpace(companyName, nameof(companyName)),
            Guard.AgainstNullOrWhiteSpace(position, nameof(position)),
            status,
            dateApplied,
            notes?.Trim());
    }

    /// <summary>
    /// Updates the application details.
    /// </summary>
    /// <param name="companyName">Updated company name.</param>
    /// <param name="position">Updated position or role.</param>
    /// <param name="status">Updated application status.</param>
    /// <param name="dateApplied">Updated application date.</param>
    /// <param name="notes">Updated notes, if any.</param>
    public void Update(
        string companyName,
        string position,
        ApplicationStatus status,
        DateOnly dateApplied,
        string? notes)
    {
        CompanyName = Guard.AgainstNullOrWhiteSpace(companyName, nameof(companyName));
        Position = Guard.AgainstNullOrWhiteSpace(position, nameof(position));
        Status = status;
        DateApplied = dateApplied;
        Notes = notes?.Trim();
        Touch();
    }

    private void Touch() => LastUpdatedOn = DateTimeOffset.UtcNow;
}

