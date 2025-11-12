using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using JobApplicationTracker.Api.Common.Json;
using JobApplicationTracker.Api.Common.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JobApplicationTracker.Api.Extensions;

public static class PresentationServiceRegistration
{
    private const string CorsPolicyName = "Frontend";

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

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
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

    public static string GetCorsPolicyName() => CorsPolicyName;
}

