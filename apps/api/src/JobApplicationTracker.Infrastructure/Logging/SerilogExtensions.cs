using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JobApplicationTracker.Infrastructure.Logging;

/// <summary>
/// Provides Serilog configuration helpers.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Configures Serilog for the host using application configuration and dependency injection.
    /// </summary>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The same host builder instance for chaining.</returns>
    public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "JobApplicationTracker")
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture);
        });
    }
}

