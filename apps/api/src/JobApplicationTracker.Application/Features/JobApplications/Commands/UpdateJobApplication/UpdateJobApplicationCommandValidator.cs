using System;
using FluentValidation;
using JobApplicationTracker.Application.Features.JobApplications.Validation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Update;

/// <summary>
/// Validates <see cref="UpdateJobApplicationCommand"/> instances to ensure updates are consistent.
/// </summary>
public sealed class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateJobApplicationCommandValidator"/> class.
    /// </summary>
    public UpdateJobApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        this.ApplyJobApplicationCommandRules(
            command => command.CompanyName,
            command => command.Position,
            command => command.Status,
            command => command.DateApplied,
            command => command.Notes);
    }
}

