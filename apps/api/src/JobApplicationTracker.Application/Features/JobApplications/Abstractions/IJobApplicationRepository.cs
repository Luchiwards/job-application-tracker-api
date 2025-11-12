using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;

namespace JobApplicationTracker.Application.Features.JobApplications.Abstractions;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(
        int id,
        bool asTracking = false,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<JobApplication> Items, int TotalCount)> SearchAsync(
        JobApplicationQueryParameters parameters,
        CancellationToken cancellationToken = default);

    Task AddAsync(JobApplication application, CancellationToken cancellationToken = default);

    Task UpdateAsync(JobApplication application, CancellationToken cancellationToken = default);

    Task DeleteAsync(JobApplication application, CancellationToken cancellationToken = default);
}

