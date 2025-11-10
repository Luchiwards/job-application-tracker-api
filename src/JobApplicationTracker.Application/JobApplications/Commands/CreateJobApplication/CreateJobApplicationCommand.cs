using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Interfaces.Repositories;
using JobApplicationTracker.Application.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using MapsterMapper;
using MediatR;

namespace JobApplicationTracker.Application.JobApplications.Commands.CreateJobApplication;

public sealed record CreateJobApplicationCommand(
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes) : IRequest<JobApplicationDto>;

public sealed class CreateJobApplicationCommandHandler
    : IRequestHandler<CreateJobApplicationCommand, JobApplicationDto>
{
    private readonly IJobApplicationRepository _repository;
    private readonly IMapper _mapper;

    public CreateJobApplicationCommandHandler(
        IJobApplicationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<JobApplicationDto> Handle(
        CreateJobApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = JobApplication.Create(
            request.CompanyName,
            request.Position,
            request.Status,
            request.DateApplied,
            request.Notes);

        await _repository.AddAsync(application, cancellationToken);

        return _mapper.Map<JobApplicationDto>(application);
    }
}

