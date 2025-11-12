using System;
using FluentValidation;
using JobApplicationTracker.Application.Features.JobApplications.Validation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Create;

public sealed class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
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

