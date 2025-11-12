using System.Reflection;
using FluentValidation;
using JobApplicationTracker.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JobApplicationTracker.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

