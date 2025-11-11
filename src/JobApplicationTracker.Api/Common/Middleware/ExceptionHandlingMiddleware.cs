using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation;
using JobApplicationTracker.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace JobApplicationTracker.Api.Common.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private static readonly Action<ILogger, string, Exception?> ValidationFailureMessage =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1, nameof(ValidationFailureMessage)),
            "Validation failure processing {Path}");

    private static readonly Action<ILogger, string, Exception?> NotFoundMessage =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2, nameof(NotFoundMessage)),
            "Resource not found processing {Path}");

    private static readonly Action<ILogger, string, Exception?> UnhandledExceptionMessage =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(3, nameof(UnhandledExceptionMessage)),
            "Unhandled exception processing {Path}");

    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        IProblemDetailsService problemDetailsService,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            ValidationFailureMessage(_logger, context.Request.Path, ex);
            var problemDetails = new HttpValidationProblemDetails(
                CreateValidationErrors(ex.Errors))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failure",
                Detail = "See errors property for additional details."
            };

            await WriteProblemDetailsAsync(context, problemDetails);
        }
        catch (NotFoundException ex)
        {
            NotFoundMessage(_logger, context.Request.Path, ex);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = ex.Message
            };

            await WriteProblemDetailsAsync(context, problemDetails);
        }
        catch (Exception ex)
        {
            UnhandledExceptionMessage(_logger, context.Request.Path, ex);
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred."
            };

            await WriteProblemDetailsAsync(context, problemDetails);
        }
    }

    private async Task WriteProblemDetailsAsync(HttpContext httpContext, ProblemDetails problemDetails)
    {
        problemDetails.Instance ??= httpContext.Request.Path;
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        var written = await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        if (!written)
        {
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static Dictionary<string, string[]> CreateValidationErrors(
        IEnumerable<FluentValidation.Results.ValidationFailure> errors)
    {
        return errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray(),
                StringComparer.Ordinal);
    }
}


