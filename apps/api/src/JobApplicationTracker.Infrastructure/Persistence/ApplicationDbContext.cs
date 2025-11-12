using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the job application tracker.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">Options used to configure the context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the job applications persisted in the data store.
    /// </summary>
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplySeedData();
        base.OnModelCreating(modelBuilder);
    }
}

