using System;
using System.Data;
using System.Linq;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobApplicationTracker.Api.Tests.Infrastructure;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureServices(services =>
        {
            RemoveService<ApplicationDbContext>(services);
            RemoveService<DbContextOptions<ApplicationDbContext>>(services);
            RemoveService<SqliteConnection>(services);

            _connection ??= new SqliteConnection("DataSource=IntegrationTests;Mode=Memory;Cache=Shared");
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            services.AddSingleton(_connection);

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var connection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(connection);
                options.EnableDetailedErrors();
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection?.Dispose();
        }
    }

    private static void RemoveService<TService>(IServiceCollection services)
    {
        var descriptors = services
            .Where(descriptor => descriptor.ServiceType == typeof(TService))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}


