using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Delete;

/// <summary>
/// Represents a request to delete an existing job application.
/// </summary>
/// <param name="Id">Identifier of the job application to delete.</param>
public sealed record DeleteJobApplicationCommand(int Id) : IRequest;

/// <summary>
/// Handles job application deletions by removing entities from the repository.
/// </summary>
public sealed class DeleteJobApplicationCommandHandler : IRequestHandler<DeleteJobApplicationCommand>
{
    private readonly IJobApplicationRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteJobApplicationCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">Repository used to locate and remove job applications.</param>
    public DeleteJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Deletes the requested job application from the data store.
    /// </summary>
    /// <param name="request">Command identifying the job application to delete.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the job application does not exist.</exception>
    public async Task Handle(DeleteJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(request.Id, asTracking: true, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("JobApplication", request.Id);
        }

        await _repository.DeleteAsync(application, cancellationToken);
    }
}

