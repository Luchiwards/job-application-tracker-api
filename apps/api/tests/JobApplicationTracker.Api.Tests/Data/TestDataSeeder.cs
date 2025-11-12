using System;
using System.Threading.Tasks;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Api.Tests.Data;

internal static class TestDataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.JobApplications.AnyAsync())
        {
            return;
        }

        var seedApplications = new[]
        {
            JobApplication.Create(
                "Contoso",
                "Backend Engineer",
                ApplicationStatus.Shortlisted,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14)),
                "Initial phone screen complete."),
            JobApplication.Create(
                "Fabrikam",
                "Full Stack Developer",
                ApplicationStatus.InProcess,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)),
                "Waiting for recruiter feedback."),
            JobApplication.Create(
                "Northwind",
                "Site Reliability Engineer",
                ApplicationStatus.Offer,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
                "Offer expires next week.")
        };

        await context.JobApplications.AddRangeAsync(seedApplications);
        await context.SaveChangesAsync();
    }
}


