using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Queries.GetById;

public sealed record GetJobApplicationByIdQuery(Guid Id) : IRequest<JobApplicationDto>;

public sealed class GetJobApplicationByIdQueryHandler
    : IRequestHandler<GetJobApplicationByIdQuery, JobApplicationDto>
{
    private readonly IJobApplicationRepository _repository;

    public GetJobApplicationByIdQueryHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

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

