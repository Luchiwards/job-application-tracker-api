using FluentAssertions;
using JobApplicationTracker.Application.Common.Exceptions;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Commands.Update;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using NSubstitute;

namespace JobApplicationTracker.Application.Tests.Features.JobApplications.Commands;

public class UpdateJobApplicationCommandHandlerTests
{
    private readonly IJobApplicationRepository _repository = Substitute.For<IJobApplicationRepository>();
    private readonly UpdateJobApplicationCommandHandler _handler;

    public UpdateJobApplicationCommandHandlerTests()
    {
        _handler = new UpdateJobApplicationCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleShouldUpdateExistingApplication()
    {
        // Arrange
        var existing = JobApplication.Create(
            "Contoso",
            "Developer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)));

        _repository.GetByIdAsync(existing.Id, true, Arg.Any<CancellationToken>())
            .Returns(existing);

        var command = new UpdateJobApplicationCommand(
            existing.Id,
            "Contoso Ltd",
            "Senior Developer",
            ApplicationStatus.InProcess,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)),
            "Second interview scheduled");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _repository.Received(1)
            .UpdateAsync(existing, Arg.Any<CancellationToken>());

        existing.CompanyName.Should().Be("Contoso Ltd");
        existing.Position.Should().Be("Senior Developer");
        existing.Status.Should().Be(ApplicationStatus.InProcess);
    }

    [Fact]
    public async Task HandleShouldThrowWhenApplicationNotFound()
    {
        // Arrange
        _repository.GetByIdAsync(Arg.Any<int>(), true, Arg.Any<CancellationToken>())
            .Returns((JobApplication?)null);

        var command = new UpdateJobApplicationCommand(
            1,
            "Missing Co",
            "Developer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow),
            null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}


