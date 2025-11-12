using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplySeedData();
        base.OnModelCreating(modelBuilder);
    }
}

