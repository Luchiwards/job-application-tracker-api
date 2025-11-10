using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Common.Interfaces.Repositories;
using JobApplicationTracker.Application.JobApplications.Models;
using MapsterMapper;
using MediatR;

namespace JobApplicationTracker.Application.JobApplications.Queries.GetJobApplicationById;

public sealed record GetJobApplicationByIdQuery(Guid Id) : IRequest<JobApplicationDto>;

public sealed class GetJobApplicationByIdQueryHandler
    : IRequestHandler<GetJobApplicationByIdQuery, JobApplicationDto>
{
    private readonly IJobApplicationRepository _repository;
    private readonly IMapper _mapper;

    public GetJobApplicationByIdQueryHandler(
        IJobApplicationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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

        return _mapper.Map<JobApplicationDto>(application);
    }
}

