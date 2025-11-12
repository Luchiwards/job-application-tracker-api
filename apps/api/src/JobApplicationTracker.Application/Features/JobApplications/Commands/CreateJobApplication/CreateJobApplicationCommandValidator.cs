using System;
using FluentValidation;
using JobApplicationTracker.Application.Features.JobApplications.Validation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Create;

/// <summary>
/// Validates <see cref="CreateJobApplicationCommand"/> instances using shared job application rules.
/// </summary>
public sealed class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateJobApplicationCommandValidator"/> class.
    /// </summary>
    public CreateJobApplicationCommandValidator()
    {
        this.ApplyJobApplicationCommandRules(
            command => command.CompanyName,
            command => command.Position,
            command => command.Status,
            command => command.DateApplied,
            command => command.Notes);
    }
}

