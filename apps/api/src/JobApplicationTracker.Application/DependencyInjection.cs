using System.Reflection;
using FluentValidation;
using JobApplicationTracker.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JobApplicationTracker.Application;

/// <summary>
/// Provides extension methods for registering application-layer services.
/// </summary>
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Registers mediators, validators, and pipeline behaviors required by the application layer.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

