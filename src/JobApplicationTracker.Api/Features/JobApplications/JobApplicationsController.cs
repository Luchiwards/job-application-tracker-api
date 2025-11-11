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

[ApiController]
[Route("api/v{version:apiVersion}/job-applications")]
[ApiVersion("1.0")]
public sealed class JobApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobApplicationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    [HttpGet("{id:guid}", Name = nameof(GetByIdAsync))]
    [ProducesResponseType(typeof(JobApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(ToResponse(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(JobApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JobApplicationResponse>> CreateAsync(
        [FromBody] CreateJobApplicationRequest request,
        CancellationToken cancellationToken)
    {
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        [FromBody] UpdateJobApplicationRequest request,
        CancellationToken cancellationToken)
    {
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
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
