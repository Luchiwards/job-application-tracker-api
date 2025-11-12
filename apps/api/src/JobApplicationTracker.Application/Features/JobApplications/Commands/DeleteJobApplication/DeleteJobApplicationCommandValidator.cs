using FluentValidation;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Delete;

/// <summary>
/// Validates <see cref="DeleteJobApplicationCommand"/> instances.
/// </summary>
public sealed class DeleteJobApplicationCommandValidator : AbstractValidator<DeleteJobApplicationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteJobApplicationCommandValidator"/> class.
    /// </summary>
    public DeleteJobApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

