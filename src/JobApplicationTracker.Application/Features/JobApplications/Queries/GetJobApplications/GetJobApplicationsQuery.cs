using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Common.Models;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetAll;

public sealed record GetJobApplicationsQuery(
    int Page = 1,
    int PageSize = 20,
    ApplicationStatus? Status = null,
    string? SearchTerm = null,
    DateOnly? AppliedFrom = null,
    DateOnly? AppliedTo = null) : IRequest<PaginatedList<JobApplicationDto>>;

public sealed class GetJobApplicationsQueryHandler
    : IRequestHandler<GetJobApplicationsQuery, PaginatedList<JobApplicationDto>>
{
    private readonly IJobApplicationRepository _repository;

    public GetJobApplicationsQueryHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<JobApplicationDto>> Handle(
        GetJobApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var criteria = new JobApplicationQueryParameters(
            request.Page,
            request.PageSize,
            request.Status,
            request.SearchTerm,
            request.AppliedFrom,
            request.AppliedTo);

        var page = await _repository.SearchAsync(criteria, cancellationToken);

        return PaginatedList.Create(
            page.Items,
            page.TotalCount,
            request.Page,
            request.PageSize,
            JobApplicationDto.FromEntity);
    }
}

