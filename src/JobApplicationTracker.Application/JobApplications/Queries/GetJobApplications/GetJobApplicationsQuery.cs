using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Models;
using JobApplicationTracker.Application.Common.Interfaces.Repositories;
using JobApplicationTracker.Application.JobApplications.Models;
using JobApplicationTracker.Domain.Enums;
using MapsterMapper;
using MediatR;

namespace JobApplicationTracker.Application.JobApplications.Queries.GetJobApplications;

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
    private readonly IMapper _mapper;

    public GetJobApplicationsQueryHandler(
        IJobApplicationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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

        var (items, totalCount) = await _repository.SearchAsync(criteria, cancellationToken);

        if (!items.Any())
        {
            return PaginatedList<JobApplicationDto>.Empty(request.Page, request.PageSize);
        }

        var dtos = items.Select(_mapper.Map<JobApplicationDto>).ToList();

        return new PaginatedList<JobApplicationDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}

