using System;
using System.Linq;
using FluentAssertions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using JobApplicationTracker.Infrastructure.Features.JobApplications;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Infrastructure.Tests.Features.JobApplications;

public class JobApplicationRepositoryTests : IAsyncLifetime, IAsyncDisposable, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _dbContext;
    private readonly JobApplicationRepository _repository;

    public JobApplicationRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new JobApplicationRepository(_dbContext);
    }

    [Fact]
    public async Task AddAsyncShouldPersistEntity()
    {
        // Arrange
        var application = JobApplication.Create(
            "Northwind",
            "Backend Engineer",
            ApplicationStatus.Applied,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)));

        // Act
        await _repository.AddAsync(application, CancellationToken.None);

        // Assert
        var stored = await _repository.GetByIdAsync(application.Id, cancellationToken: CancellationToken.None);
        stored.Should().NotBeNull();
        stored!.CompanyName.Should().Be("Northwind");
    }

    [Fact]
    public async Task SearchAsyncShouldRespectPagingAndFilters()
    {
        // Arrange
        var statuses = Enum.GetValues<ApplicationStatus>();
        foreach (var index in Enumerable.Range(0, 15))
        {
            var entity = JobApplication.Create(
                $"Company {index}",
                $"Role {index}",
                statuses[index % statuses.Length],
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-index)));
            await _repository.AddAsync(entity, CancellationToken.None);
        }

        var parameters = new JobApplicationQueryParameters(
            Page: 2,
            PageSize: 5,
            Status: ApplicationStatus.Applied,
            SearchTerm: "Company",
            AppliedFrom: null,
            AppliedTo: null);

        // Act
        var (items, totalCount) = await _repository.SearchAsync(parameters, CancellationToken.None);

        // Assert
        totalCount.Should().BeGreaterThan(0);
        items.Count.Should().BeLessThanOrEqualTo(parameters.PageSize);
        items.Should().OnlyContain(item => item.Status == ApplicationStatus.Applied);
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await DisposeAsyncCore();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        DisposeAsyncCore().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    private async Task DisposeAsyncCore()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _connection.DisposeAsync();
    }
}

