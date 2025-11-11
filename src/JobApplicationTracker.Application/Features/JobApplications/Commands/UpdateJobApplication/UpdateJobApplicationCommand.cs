using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Domain.Enums;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Update;

public sealed record UpdateJobApplicationCommand(
    int Id,
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

        application.Update(
            request.CompanyName,
            request.Position,
            request.Status,
            request.DateApplied,
            request.Notes);

        await _repository.UpdateAsync(application, cancellationToken);
    }
}

