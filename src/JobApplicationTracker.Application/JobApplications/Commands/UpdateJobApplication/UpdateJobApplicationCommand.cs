using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Common.Interfaces.Repositories;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.JobApplications.Commands.UpdateJobApplication;

public sealed record UpdateJobApplicationCommand(
    Guid Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateOnly DateApplied,
    string? Notes) : IRequest;

public sealed class UpdateJobApplicationCommandHandler : IRequestHandler<UpdateJobApplicationCommand>
{
    private readonly IJobApplicationRepository _repository;

    public UpdateJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(request.Id, asTracking: true, cancellationToken);

        if (application is null)
        {
            throw new NotFoundException("JobApplication", request.Id);
        }

        application.UpdateDetails(request.CompanyName, request.Position, request.DateApplied, request.Notes);
        application.UpdateStatus(request.Status);

        await _repository.UpdateAsync(application, cancellationToken);
    }
}

