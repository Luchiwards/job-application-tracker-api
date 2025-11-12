using System;
using System.Linq.Expressions;
using FluentValidation;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Features.JobApplications.Validation;

public static class JobApplicationCommandValidationExtensions
{
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


