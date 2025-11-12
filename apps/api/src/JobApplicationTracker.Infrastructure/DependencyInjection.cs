using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Infrastructure.Features.JobApplications;
using JobApplicationTracker.Infrastructure.Options;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobApplicationTracker.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure-layer services.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Registers infrastructure services including the database context, repositories, and health checks.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="configuration">Configuration used to resolve database settings.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var connectionString = ResolveConnectionString(configuration, databaseOptions);

            switch (databaseOptions.Provider.ToLowerInvariant())
            {
                case "sqlite":
                default:
                    options.UseSqlite(connectionString);
                    break;
            }

            options.EnableDetailedErrors();

            if (databaseOptions.EnableSensitiveLogging)
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }

    /// <summary>
    /// Applies any pending EF Core migrations at application startup.
    /// </summary>
    /// <param name="services">Service provider used to create a scoped <see cref="ApplicationDbContext"/>.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous initialization.</returns>
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }

    private static string ResolveConnectionString(IConfiguration configuration, DatabaseOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return options.ConnectionString;
        }

        var connectionString = configuration.GetConnectionString("Default");
        return string.IsNullOrWhiteSpace(connectionString)
            ? "Data Source=job-application-tracker.db"
            : connectionString;
    }
}

