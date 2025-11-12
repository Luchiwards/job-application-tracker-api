using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetById;

/// <summary>
/// Represents a request to retrieve a single job application by its identifier.
/// </summary>
/// <param name="Id">Identifier of the job application to retrieve.</param>
public sealed record GetJobApplicationByIdQuery(int Id) : IRequest<JobApplicationDto>;

/// <summary>
/// Handles retrieval of a single job application.
/// </summary>
public sealed class GetJobApplicationByIdQueryHandler
    : IRequestHandler<GetJobApplicationByIdQuery, JobApplicationDto>
{
    private readonly IJobApplicationRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetJobApplicationByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">Repository used to load job applications.</param>
    public GetJobApplicationByIdQueryHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves the job application identified by the request.
    /// </summary>
    /// <param name="request">Query describing which job application to retrieve.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The job application in DTO form.</returns>
    /// <exception cref="NotFoundException">Thrown when the job application does not exist.</exception>
    public async Task<JobApplicationDto> Handle(
        GetJobApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(request.Id, asTracking: false, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("JobApplication", request.Id);
        }

        return JobApplicationDto.FromEntity(application);
    }
}

