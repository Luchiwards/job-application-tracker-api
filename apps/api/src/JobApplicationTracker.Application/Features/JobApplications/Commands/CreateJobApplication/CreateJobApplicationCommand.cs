using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Create;

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

    public CreateJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
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

        return JobApplicationDto.FromEntity(application);
    }
}

