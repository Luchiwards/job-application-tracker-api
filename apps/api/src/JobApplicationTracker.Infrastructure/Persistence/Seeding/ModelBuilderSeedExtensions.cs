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
            });
    }
}


