using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Update;

/// <summary>
/// Represents a request to update an existing job application.
/// </summary>
/// <param name="Id">Identifier of the job application to update.</param>
/// <param name="CompanyName">Updated company name.</param>
/// <param name="Position">Updated position or role.</param>
/// <param name="Status">New application status.</param>
/// <param name="DateApplied">Date the application was originally submitted.</param>
/// <param name="Notes">Optional notes about the application.</param>
public sealed record UpdateJobApplicationCommand(
    int Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes) : IRequest;

/// <summary>
/// Handles updates to existing job applications.
/// </summary>
public sealed class UpdateJobApplicationCommandHandler : IRequestHandler<UpdateJobApplicationCommand>
{
    private readonly IJobApplicationRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateJobApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">Repository used to retrieve and persist job applications.</param>
    public UpdateJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Applies updates to a job application and persists the changes.
    /// </summary>
    /// <param name="request">Command describing the updates to apply.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the job application does not exist.</exception>
    public async Task Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(request.Id, asTracking: true, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("JobApplication", request.Id);
        }

        application.Update(
            request.CompanyName,
            request.Position,
            request.Status,
            request.DateApplied,
            request.Notes);

        await _repository.UpdateAsync(application, cancellationToken);
    }
}

