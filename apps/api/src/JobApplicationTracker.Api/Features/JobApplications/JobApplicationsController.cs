using JobApplicationTracker.Api.Common.Pagination;
using JobApplicationTracker.Api.Features.JobApplications.Contracts;
using JobApplicationTracker.Application.Features.JobApplications.Commands.Create;
using JobApplicationTracker.Application.Features.JobApplications.Commands.Delete;
using JobApplicationTracker.Application.Features.JobApplications.Commands.Update;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Application.Features.JobApplications.Queries.GetById;
using JobApplicationTracker.Application.Features.JobApplications.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Features.JobApplications;

/// <summary>
/// Provides endpoints for managing job applications including listing, creating, updating, and deleting entries.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/job-applications")]
[ApiVersion("1.0")]
[Produces("application/json")]
public sealed class JobApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobApplicationsController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator used to dispatch job application commands and queries.</param>
    public JobApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of job applications matching the supplied filters.
    /// </summary>
    /// <param name="request">Query parameters to filter and paginate job applications.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated collection of job applications.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<JobApplicationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<JobApplicationResponse>>> GetAsync(
        [FromQuery] GetJobApplicationsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationsQuery(
            request.Page,
            request.PageSize,
            request.Status,
            request.SearchTerm,
            request.AppliedFrom,
            request.AppliedTo);

        var result = await _mediator.Send(query, cancellationToken);
        var response = PaginatedResponseFactory.From(result, ToResponse);

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a single job application by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the job application.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested job application if found.</returns>
    [HttpGet("{id:int}", Name = nameof(GetByIdAsync))]
    [ProducesResponseType(typeof(JobApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationResponse>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(ToResponse(result));
    }

    /// <summary>
    /// Creates a new job application.
    /// </summary>
    /// <param name="request">Details of the job application to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created job application.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(JobApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JobApplicationResponse>> CreateAsync(
        [FromBody] CreateJobApplicationRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid payload",
                Detail = "Request body is required.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var command = new CreateJobApplicationCommand(
            request.CompanyName,
            request.Position,
            request.Status,
            request.DateApplied,
            request.Notes);

        var result = await _mediator.Send(command, cancellationToken);
        var response = ToResponse(result);

        var routeVersion = HttpContext.Request.RouteValues.TryGetValue("version", out var value)
            ? value?.ToString()
            : HttpContext.GetRequestedApiVersion()?.ToString();

        return CreatedAtRoute(
            nameof(GetByIdAsync),
            new { id = response.Id, version = routeVersion ?? "1" },
            response);
    }

    /// <summary>
    /// Updates an existing job application.
    /// </summary>
    /// <param name="id">Identifier of the job application to update.</param>
    /// <param name="request">Updated job application values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content when the operation succeeds.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] UpdateJobApplicationRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid payload",
                Detail = "Request body is required.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var command = new UpdateJobApplicationCommand(
            id,
            request.CompanyName,
            request.Position,
            request.Status,
            request.DateApplied,
            request.Notes);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Permanently deletes a job application.
    /// </summary>
    /// <param name="id">Identifier of the job application to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>No content when the job application is removed.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteJobApplicationCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
    private static JobApplicationResponse ToResponse(JobApplicationDto dto) =>
        new(
            dto.Id,
            dto.CompanyName,
            dto.Position,
            dto.Status,
            dto.DateApplied,
            dto.Notes,
            dto.LastUpdatedOn);
}
