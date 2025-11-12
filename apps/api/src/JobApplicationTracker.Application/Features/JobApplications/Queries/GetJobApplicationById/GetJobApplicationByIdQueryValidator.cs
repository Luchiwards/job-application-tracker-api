using FluentValidation;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetById;

/// <summary>
/// Validates <see cref="GetJobApplicationByIdQuery"/> instances.
/// </summary>
public sealed class GetJobApplicationByIdQueryValidator : AbstractValidator<GetJobApplicationByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetJobApplicationByIdQueryValidator"/> class.
    /// </summary>
    public GetJobApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

