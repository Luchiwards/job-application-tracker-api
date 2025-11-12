using FluentValidation;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetById;

public sealed class GetJobApplicationByIdQueryValidator : AbstractValidator<GetJobApplicationByIdQuery>
{
    public GetJobApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

