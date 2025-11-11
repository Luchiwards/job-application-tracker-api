using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Application.Features.JobApplications.Abstractions;
using JobApplicationTracker.Application.Features.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Infrastructure.Features.JobApplications;

public sealed class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JobApplicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<JobApplication?> GetByIdAsync(
        Guid id,
        bool asTracking = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.JobApplications.AsQueryable();

        if (!asTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(application => application.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<JobApplication> Items, int TotalCount)> SearchAsync(
        JobApplicationQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.JobApplications.AsNoTracking();

        if (parameters.Status.HasValue)
        {
            query = query.Where(application => application.Status == parameters.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            var term = parameters.SearchTerm.Trim();
            query = query.Where(application =>
                EF.Functions.Like(application.CompanyName, $"%{term}%") ||
                EF.Functions.Like(application.Position, $"%{term}%"));
        }

        if (parameters.AppliedFrom.HasValue)
        {
            query = query.Where(application => application.DateApplied >= parameters.AppliedFrom.Value);
        }

        if (parameters.AppliedTo.HasValue)
        {
            query = query.Where(application => application.DateApplied <= parameters.AppliedTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return (Array.Empty<JobApplication>(), 0);
        }

        var items = await query
            .OrderByDescending(application => application.DateApplied)
            .ThenBy(application => application.CompanyName)
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        await _dbContext.JobApplications.AddAsync(application, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        _dbContext.JobApplications.Update(application);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        _dbContext.JobApplications.Remove(application);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

