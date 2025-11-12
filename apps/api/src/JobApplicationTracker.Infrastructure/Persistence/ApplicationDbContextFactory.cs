using System;
using System.IO;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JobApplicationTracker.Infrastructure.Persistence;

/// <summary>
/// Creates <see cref="ApplicationDbContext"/> instances for design-time tooling such as migrations.
/// </summary>
public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new <see cref="ApplicationDbContext"/> configured for tooling scenarios.
    /// </summary>
    /// <param name="args">Command line arguments supplied by tooling.</param>
    /// <returns>A configured <see cref="ApplicationDbContext"/> instance.</returns>
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

