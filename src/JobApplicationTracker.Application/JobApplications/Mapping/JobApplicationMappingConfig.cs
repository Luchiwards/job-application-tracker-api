using JobApplicationTracker.Application.JobApplications.Commands.CreateJobApplication;
using JobApplicationTracker.Application.JobApplications.Models;
using JobApplicationTracker.Domain.Entities;
using Mapster;

namespace JobApplicationTracker.Application.JobApplications.Mapping;

public sealed class JobApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JobApplication, JobApplicationDto>();

        config.NewConfig<CreateJobApplicationCommand, JobApplication>()
            .Ignore(dest => dest.Id);
    }
}

