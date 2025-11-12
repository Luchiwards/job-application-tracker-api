using FluentValidation;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetAll;

/// <summary>
/// Validates <see cref="GetJobApplicationsQuery"/> instances.
/// </summary>
public sealed class GetJobApplicationsQueryValidator : AbstractValidator<GetJobApplicationsQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetJobApplicationsQueryValidator"/> class.
    /// </summary>
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

