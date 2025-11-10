using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Application.JobApplications.Models;

namespace JobApplicationTracker.Application.Common.Interfaces.Repositories;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(
        Guid id,
        bool asTracking = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobApplication>> ListAsync(CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<JobApplication> Items, int TotalCount)> SearchAsync(
        JobApplicationQueryParameters parameters,
        CancellationToken cancellationToken = default);

    Task AddAsync(JobApplication application, CancellationToken cancellationToken = default);

    Task UpdateAsync(JobApplication application, CancellationToken cancellationToken = default);

    Task DeleteAsync(JobApplication application, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

