using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using JobApplicationTracker.Api.Common.Json;
using JobApplicationTracker.Api.Common.Middleware;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

namespace JobApplicationTracker.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring the API presentation layer, including controllers, versioning, and documentation.
/// </summary>
public static class PresentationServiceRegistration
{
    private const string CorsPolicyName = "Frontend";

    /// <summary>
    /// Registers presentation-layer services such as controllers, middleware, CORS, API versioning, and Swagger support.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The same service collection to enable fluent configuration.</returns>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddProblemDetails();
        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddCors(options =>
        {
            options.AddPolicy(
                CorsPolicyName,
                policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Job Application Tracker API",
                Version = "v1",
                Description = "API for managing job application tracking."
            });

            options.MapType<ApplicationStatus>(() => CreateApplicationStatusSchema());

            var xmlAssemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(JobApplicationDto).Assembly,
                typeof(ApplicationStatus).Assembly
            }
            .Distinct();

            foreach (var assembly in xmlAssemblies)
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            }
        });

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }

    /// <summary>
    /// Gets the name of the canonical CORS policy used by the API.
    /// </summary>
    /// <returns>The configured CORS policy name.</returns>
    public static string GetCorsPolicyName() => CorsPolicyName;

    private static OpenApiSchema CreateApplicationStatusSchema()
    {
        var description = new StringBuilder()
            .AppendLine("Lifecycle status of a job application. Allowed values:")
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.Applied}: Initial submission received."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.Shortlisted}: Selected for further consideration."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.Interview}: Interview scheduled or in progress."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.InProcess}: Awaiting a decision after interviews."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.Offer}: Offer extended by the company."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.Declined}: Candidate declined or was rejected."))
            .AppendLine(FormattableString.Invariant($"- {ApplicationStatus.PositionClosed}: Hiring process closed without offer."))
            .ToString().TrimEnd();

        return new OpenApiSchema
        {
            Type = "string",
            Description = description,
            Enum = Enum.GetNames(typeof(ApplicationStatus))
                .Select(name => (IOpenApiAny)new OpenApiString(name))
                .ToList()
        };
    }
}

