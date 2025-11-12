using System;
using System.IO;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JobApplicationTracker.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=job-application-tracker.db";

        optionsBuilder.UseSqlite(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

