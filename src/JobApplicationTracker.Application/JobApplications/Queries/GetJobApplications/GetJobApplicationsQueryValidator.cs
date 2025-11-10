using FluentValidation;

namespace JobApplicationTracker.Application.JobApplications.Queries.GetJobApplications;

public sealed class GetJobApplicationsQueryValidator : AbstractValidator<GetJobApplicationsQuery>
{
    public GetJobApplicationsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => x.SearchTerm is not null);

        RuleFor(x => x.AppliedTo)
            .GreaterThanOrEqualTo(x => x.AppliedFrom!.Value)
            .When(x => x.AppliedFrom.HasValue && x.AppliedTo.HasValue)
            .WithMessage("Applied to date must be greater than or equal to applied from date.");
    }
}

