using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Interfaces.Repositories;
using JobApplicationTracker.Infrastructure.Options;
using JobApplicationTracker.Infrastructure.Persistence;
using JobApplicationTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobApplicationTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
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

