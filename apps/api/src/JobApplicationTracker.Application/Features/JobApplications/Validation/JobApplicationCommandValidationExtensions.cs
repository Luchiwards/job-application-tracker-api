using System;
using System.Linq.Expressions;
using FluentValidation;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Validation;

/// <summary>
/// Provides shared validation rules for job application commands.
/// </summary>
public static class JobApplicationCommandValidationExtensions
{
    /// <summary>
    /// Applies common validation rules for job application commands to the supplied validator.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command being validated.</typeparam>
    /// <param name="validator">Validator that receives the rules.</param>
    /// <param name="companyName">Expression selecting the company name property.</param>
    /// <param name="position">Expression selecting the position property.</param>
    /// <param name="status">Expression selecting the status property.</param>
    /// <param name="dateApplied">Expression selecting the date applied property.</param>
    /// <param name="notes">Expression selecting the notes property.</param>
    public static void ApplyJobApplicationCommandRules<TCommand>(
        this AbstractValidator<TCommand> validator,
        Expression<Func<TCommand, string>> companyName,
        Expression<Func<TCommand, string>> position,
        Expression<Func<TCommand, ApplicationStatus>> status,
        Expression<Func<TCommand, DateOnly>> dateApplied,
        Expression<Func<TCommand, string?>> notes)
    {
        validator.RuleFor(companyName)
            .NotEmpty()
            .MaximumLength(200);

        validator.RuleFor(position)
            .NotEmpty()
            .MaximumLength(200);

        validator.RuleFor(status)
            .IsInEnum();

        validator.RuleFor(dateApplied)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date applied cannot be in the future.");

        var notesAccessor = notes.Compile();
        validator.RuleFor(notes)
            .MaximumLength(2000)
            .When(command => notesAccessor(command) is not null);
    }
}


