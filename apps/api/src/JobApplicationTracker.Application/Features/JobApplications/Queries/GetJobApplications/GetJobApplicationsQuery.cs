using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Common.Models;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetAll;

/// <summary>
/// Represents a request to retrieve a paginated list of job applications.
/// </summary>
/// <param name="Page">Page number to retrieve (1-based).</param>
/// <param name="PageSize">Number of results per page.</param>
/// <param name="Status">Optional status filter.</param>
/// <param name="SearchTerm">Optional search term applied to company or position.</param>
/// <param name="AppliedFrom">Lower bound for the application date filter.</param>
/// <param name="AppliedTo">Upper bound for the application date filter.</param>
public sealed record GetJobApplicationsQuery(
    int Page = 1,
    int PageSize = 20,
    ApplicationStatus? Status = null,
    string? SearchTerm = null,
    DateOnly? AppliedFrom = null,
    DateOnly? AppliedTo = null) : IRequest<PaginatedList<JobApplicationDto>>;

/// <summary>
/// Handles retrieval of paged job application results.
/// </summary>
public sealed class GetJobApplicationsQueryHandler
    : IRequestHandler<GetJobApplicationsQuery, PaginatedList<JobApplicationDto>>
{
    private readonly IJobApplicationRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetJobApplicationsQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Repository used to query job applications.</param>
    public GetJobApplicationsQueryHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves a paginated set of job applications that match the supplied query.
    /// </summary>
    /// <param name="request">Query describing the filters to apply.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A paginated list of job application DTOs.</returns>
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

