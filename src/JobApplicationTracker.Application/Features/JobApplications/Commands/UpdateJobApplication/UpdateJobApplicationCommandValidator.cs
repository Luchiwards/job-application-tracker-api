using System;
using FluentValidation;
using JobApplicationTracker.Application.Features.JobApplications.Validation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Update;

public sealed class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
{
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

