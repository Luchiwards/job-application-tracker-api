using System;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using MediatR;

namespace JobApplicationTracker.Application.Features.JobApplications.Commands.Delete;

public sealed record DeleteJobApplicationCommand(int Id) : IRequest;

public sealed class DeleteJobApplicationCommandHandler : IRequestHandler<DeleteJobApplicationCommand>
{
    private readonly IJobApplicationRepository _repository;

    public DeleteJobApplicationCommandHandler(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

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

