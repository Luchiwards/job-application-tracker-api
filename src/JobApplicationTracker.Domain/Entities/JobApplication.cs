using System;

using JobApplicationTracker.Domain.Common;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Domain.Entities;

public sealed class JobApplication : Entity
{
    private JobApplication(Guid id,
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
        Guid? id = null)
    {
        return new JobApplication(
            id ?? Guid.NewGuid(),
            Guard.AgainstNullOrWhiteSpace(companyName, nameof(companyName)),
            Guard.AgainstNullOrWhiteSpace(position, nameof(position)),
            status,
            dateApplied,
            notes?.Trim());
    }

    public void UpdateDetails(string companyName, string position, DateOnly dateApplied, string? notes)
    {
        CompanyName = Guard.AgainstNullOrWhiteSpace(companyName, nameof(companyName));
        Position = Guard.AgainstNullOrWhiteSpace(position, nameof(position));
        DateApplied = dateApplied;
        Notes = notes?.Trim();
        Touch();
    }

    public void UpdateStatus(ApplicationStatus status)
    {
        Status = status;
        Touch();
    }

    private void Touch() => LastUpdatedOn = DateTimeOffset.UtcNow;
}

