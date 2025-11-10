using FluentValidation;

namespace JobApplicationTracker.Application.JobApplications.Queries.GetJobApplicationById;

public sealed class GetJobApplicationByIdQueryValidator : AbstractValidator<GetJobApplicationByIdQuery>
{
    public GetJobApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

