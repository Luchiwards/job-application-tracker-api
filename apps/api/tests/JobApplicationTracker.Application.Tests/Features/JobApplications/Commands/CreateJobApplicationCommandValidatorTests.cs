using FluentAssertions;
using JobApplicationTracker.Application.Features.JobApplications.Commands.Create;
using JobApplicationTracker.Domain.Enums;

namespace JobApplicationTracker.Application.Tests.Features.JobApplications.Commands;

public class CreateJobApplicationCommandValidatorTests
{
    private readonly CreateJobApplicationCommandValidator _validator = new();

    [Fact]
    public void ValidateShouldPassForValidRequest()
    {
        // Arrange
        var command = new CreateJobApplicationCommand(
            "Contoso",
            "Senior Developer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
            "Exciting opportunity");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateShouldFailWhenCompanyNameMissing()
    {
        // Arrange
        var command = new CreateJobApplicationCommand(
            string.Empty,
            "Engineer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow),
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle(failure => failure.PropertyName == nameof(CreateJobApplicationCommand.CompanyName));
    }

    [Fact]
    public void ValidateShouldFailWhenDateAppliedInFuture()
    {
        // Arrange
        var command = new CreateJobApplicationCommand(
            "Fabrikam",
            "Engineer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(failure => failure.PropertyName == nameof(CreateJobApplicationCommand.DateApplied));
    }
}


