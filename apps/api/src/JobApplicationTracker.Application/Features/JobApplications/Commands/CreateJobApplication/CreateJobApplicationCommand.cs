using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Create;

/// <summary>
/// Represents a request to create a new job application.
/// </summary>
/// <param name="CompanyName">Name of the company the application targets.</param>
/// <param name="Position">Role or position applied for.</param>
/// <param name="Status">Initial status of the application.</param>
/// <param name="DateApplied">Date the application was submitted.</param>
/// <param name="Notes">Optional notes associated with the application.</param>
public sealed record CreateJobApplicationCommand(
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes) : IRequest<JobApplicationDto>;

/// <summary>
/// Handles creation of job applications by persisting new entities.
/// </summary>
public sealed class CreateJobApplicationCommandHandler
    : IRequestHandler<CreateJobApplicationCommand, JobApplicationDto>
{
    private readonly IJobApplicationRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateJobApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">Repository used to persist job applications.</param>
    public CreateJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Persists the job application described in the request and returns its DTO representation.
    /// </summary>
    /// <param name="request">Command describing the job application to create.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The created job application.</returns>
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

