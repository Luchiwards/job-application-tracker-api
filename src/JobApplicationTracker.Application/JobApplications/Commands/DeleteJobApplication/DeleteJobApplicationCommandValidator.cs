using FluentValidation;

namespace JobApplicationTracker.Application.JobApplications.Commands.DeleteJobApplication;

public sealed class DeleteJobApplicationCommandValidator : AbstractValidator<DeleteJobApplicationCommand>
{
    public DeleteJobApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

