using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using JobApplicationTracker.Api.Common.Pagination;
using JobApplicationTracker.Api.Features.JobApplications.Contracts;
using JobApplicationTracker.Api.Tests.Infrastructure;
using JobApplicationTracker.Domain.Enums;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobApplicationTracker.Api.Tests.Features.JobApplications;

public sealed class JobApplicationsCrudTests : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    private const string BaseUrl = "/api/v1.0/job-applications";
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = null,
        PropertyNameCaseInsensitive = true
    };

    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public JobApplicationsCrudTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        pendingMigrations.Should().BeEmpty("database schema should be up to date for integration tests");
        await dbContext.JobApplications.ExecuteDeleteAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task JobApplicationsCrudFlowSucceeds()
    {
        var createRequest = new CreateJobApplicationRequest
        {
            CompanyName = "Contoso",
            Position = "Backend Engineer",
            Status = ApplicationStatus.Applied,
            DateApplied = DateOnly.FromDateTime(DateTime.UtcNow.Date),
            Notes = "Resume submitted"
        };

        var createResponse = await _client.PostAsJsonAsync(
            $"{BaseUrl}?api-version=1.0",
            createRequest,
            SerializerOptions);

        var createPayload = await createResponse.Content.ReadAsStringAsync();
        createPayload.Should().NotBeNullOrWhiteSpace();

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"payload: {createPayload}");

        var created = JsonSerializer.Deserialize<JobApplicationResponse>(createPayload, SerializerOptions);
        created.Should().NotBeNull();
        var createdApplication = created!;

        createdApplication.CompanyName.Should().Be(
            createRequest.CompanyName,
            $"payload: {createPayload}");
        createdApplication.Position.Should().Be(createRequest.Position);
        createdApplication.Status.Should().Be(createRequest.Status);

        var listResponse = await _client.GetAsync($"{BaseUrl}?page=1&pageSize=10&api-version=1.0");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await listResponse.Content.ReadFromJsonAsync<PaginatedResponse<JobApplicationResponse>>(SerializerOptions);
        list.Should().NotBeNull();
        list!.Items.Should().ContainSingle(item => item.Id == createdApplication.Id);

        var getByIdResponse = await _client.GetAsync($"{BaseUrl}/{createdApplication.Id}?api-version=1.0");
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await getByIdResponse.Content.ReadFromJsonAsync<JobApplicationResponse>(SerializerOptions);
        fetched.Should().BeEquivalentTo(createdApplication);

        var updateRequest = new UpdateJobApplicationRequest
        {
            CompanyName = createRequest.CompanyName,
            Position = "Senior Backend Engineer",
            Status = ApplicationStatus.Interviewing,
            DateApplied = createRequest.DateApplied,
            Notes = "First interview completed"
        };

        var updateResponse = await _client.PutAsJsonAsync(
            $"{BaseUrl}/{createdApplication.Id}?api-version=1.0",
            updateRequest,
            SerializerOptions);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedResponse = await _client.GetAsync($"{BaseUrl}/{createdApplication.Id}?api-version=1.0");
        updatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await updatedResponse.Content.ReadFromJsonAsync<JobApplicationResponse>(SerializerOptions);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be(updateRequest.CompanyName);
        updated.Position.Should().Be(updateRequest.Position);
        updated.Status.Should().Be(updateRequest.Status);
        updated.Notes.Should().Be(updateRequest.Notes);
        updated.LastUpdatedOn.Should().BeOnOrAfter(createdApplication.LastUpdatedOn);

        var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{createdApplication.Id}?api-version=1.0");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var notFoundResponse = await _client.GetAsync($"{BaseUrl}/{createdApplication.Id}?api-version=1.0");
        var notFoundPayload = await notFoundResponse.Content.ReadAsStringAsync();
        notFoundResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, $"payload: {notFoundPayload}");
    }
}


