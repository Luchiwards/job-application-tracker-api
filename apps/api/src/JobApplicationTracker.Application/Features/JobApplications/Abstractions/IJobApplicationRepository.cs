using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;

namespace JobApplicationTracker.Application.Features.JobApplications.Abstractions;

/// <summary>
/// Defines persistence operations for managing job application aggregates.
/// </summary>
public interface IJobApplicationRepository
{
    /// <summary>
    /// Retrieves a job application by its identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the job application.</param>
    /// <param name="asTracking">Indicates whether the entity should be tracked by the change tracker.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The matching job application if found; otherwise, <c>null</c>.</returns>
    Task<JobApplication?> GetByIdAsync(
        int id,
        bool asTracking = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for job applications matching the supplied parameters.
    /// </summary>
    /// <param name="parameters">Filtering and pagination parameters.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A tuple containing the matching <see cref="JobApplication"/> items and the total count of results.
    /// </returns>
    Task<(IReadOnlyList<JobApplication> Items, int TotalCount)> SearchAsync(
        JobApplicationQueryParameters parameters,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new job application to the data store.
    /// </summary>
    /// <param name="application">Application to be added.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task AddAsync(JobApplication application, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing job application in the data store.
    /// </summary>
    /// <param name="application">Application instance containing updated values.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task UpdateAsync(JobApplication application, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a job application from the data store.
    /// </summary>
    /// <param name="application">Application to be removed.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    Task DeleteAsync(JobApplication application, CancellationToken cancellationToken = default);
}

