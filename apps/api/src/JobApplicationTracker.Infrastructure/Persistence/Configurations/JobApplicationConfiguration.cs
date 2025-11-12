using System;
using JobApplicationTracker.Domain.Entities;
using JobApplicationTracker.Domain.Enums;
using JobApplicationTracker.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationTracker.Infrastructure.Persistence.Configurations;

public sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasKey(application => application.Id);

        builder.Property(application => application.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(application => application.Position)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(application => application.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(ApplicationStatus.Applied);

        builder.Property(application => application.DateApplied)
            .HasConversion(DateTimeConverters.DateOnlyToDateTime)
            .HasColumnType("TEXT")
            .IsRequired();

        builder.Property(application => application.Notes)
            .HasMaxLength(2000);

        builder.Property(application => application.LastUpdatedOn)
            .HasConversion(DateTimeConverters.UtcDateTimeOffset)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(application => new { application.CompanyName, application.Position });
    }
}

