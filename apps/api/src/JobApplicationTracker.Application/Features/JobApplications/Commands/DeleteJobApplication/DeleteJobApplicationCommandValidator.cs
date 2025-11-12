using FluentValidation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Delete;

public sealed class DeleteJobApplicationCommandValidator : AbstractValidator<DeleteJobApplicationCommand>
{
    public DeleteJobApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

