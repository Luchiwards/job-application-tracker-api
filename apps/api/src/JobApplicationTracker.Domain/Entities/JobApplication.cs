using System;

using JobApplicationTracker.Domain.Common;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Domain.Entities;

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

    public string CompanyName { get; private set; } = null!;

    public string Position { get; private set; } = null!;

    public ApplicationStatus Status { get; private set; }

    public DateOnly DateApplied { get; private set; }

    public string? Notes { get; private set; }

    public DateTimeOffset LastUpdatedOn { get; private set; }

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

