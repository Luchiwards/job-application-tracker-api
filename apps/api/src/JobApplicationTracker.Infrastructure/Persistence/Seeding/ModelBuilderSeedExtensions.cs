using System;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Infrastructure.Persistence.Seeding;

internal static class ModelBuilderSeedExtensions
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>().HasData(
            new
            {
                Id = 1,
                CompanyName = "Google",
                Position = "Backend Engineer",
                Status = ApplicationStatus.Shortlisted,
                DateApplied = new DateOnly(2025, 1, 5),
                Notes = "Initial phone screen complete.",
                LastUpdatedOn = new DateTimeOffset(2025, 1, 15, 8, 30, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 2,
                CompanyName = "Apple",
                Position = "Full Stack Developer",
                Status = ApplicationStatus.InProcess,
                DateApplied = new DateOnly(2025, 2, 10),
                Notes = "Waiting for recruiter feedback.",
                LastUpdatedOn = new DateTimeOffset(2025, 2, 11, 10, 0, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 3,
                CompanyName = "Microsoft",
                Position = "Site Reliability Engineer",
                Status = ApplicationStatus.Offer,
                DateApplied = new DateOnly(2024, 12, 20),
                Notes = "Offer expires next week.",
                LastUpdatedOn = new DateTimeOffset(2025, 1, 2, 14, 15, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 4,
                CompanyName = "Amazon",
                Position = "Cloud Support Engineer",
                Status = ApplicationStatus.Applied,
                DateApplied = new DateOnly(2025, 3, 5),
                Notes = "Preparing technical assessment.",
                LastUpdatedOn = new DateTimeOffset(2025, 3, 6, 9, 15, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 5,
                CompanyName = "Meta",
                Position = "Machine Learning Engineer",
                Status = ApplicationStatus.Interview,
                DateApplied = new DateOnly(2025, 1, 28),
                Notes = "Panel interview scheduled for next week.",
                LastUpdatedOn = new DateTimeOffset(2025, 2, 5, 16, 45, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 6,
                CompanyName = "Netflix",
                Position = "Security Engineer",
                Status = ApplicationStatus.InProcess,
                DateApplied = new DateOnly(2025, 2, 20),
                Notes = (string?)null,
                LastUpdatedOn = new DateTimeOffset(2025, 2, 25, 11, 0, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 7,
                CompanyName = "Salesforce",
                Position = "DevOps Engineer",
                Status = ApplicationStatus.Shortlisted,
                DateApplied = new DateOnly(2025, 1, 18),
                Notes = "Awaiting hiring manager feedback.",
                LastUpdatedOn = new DateTimeOffset(2025, 1, 25, 13, 30, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 8,
                CompanyName = "Atlassian",
                Position = "Frontend Developer",
                Status = ApplicationStatus.Declined,
                DateApplied = new DateOnly(2024, 11, 29),
                Notes = "Received polite decline.",
                LastUpdatedOn = new DateTimeOffset(2024, 12, 10, 8, 20, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 9,
                CompanyName = "Adobe",
                Position = "Product Designer",
                Status = ApplicationStatus.Applied,
                DateApplied = new DateOnly(2025, 3, 1),
                Notes = "Portfolio submitted.",
                LastUpdatedOn = new DateTimeOffset(2025, 3, 2, 10, 10, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 10,
                CompanyName = "Shopify",
                Position = "Infrastructure Engineer",
                Status = ApplicationStatus.Interview,
                DateApplied = new DateOnly(2025, 2, 5),
                Notes = "On-site scheduled.",
                LastUpdatedOn = new DateTimeOffset(2025, 2, 18, 15, 45, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 11,
                CompanyName = "Stripe",
                Position = "Payments Engineer",
                Status = ApplicationStatus.Offer,
                DateApplied = new DateOnly(2024, 12, 1),
                Notes = "Reviewing offer details.",
                LastUpdatedOn = new DateTimeOffset(2025, 1, 10, 17, 0, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 12,
                CompanyName = "Tesla",
                Position = "Automation Engineer",
                Status = ApplicationStatus.Applied,
                DateApplied = new DateOnly(2025, 3, 12),
                Notes = (string?)null,
                LastUpdatedOn = new DateTimeOffset(2025, 3, 13, 7, 50, 0, TimeSpan.Zero)
            },
            new
            {
                Id = 13,
                CompanyName = "Airbnb",
                Position = "Platform Engineer",
                Status = ApplicationStatus.PositionClosed,
                DateApplied = new DateOnly(2024, 10, 15),
                Notes = "Role closed before interview.",
                LastUpdatedOn = new DateTimeOffset(2024, 11, 1, 12, 0, 0, TimeSpan.Zero)
            });
    }
}


