## Job Application Tracker API

Clean Architecture ASP.NET Core Web API hosted under `apps/api`.

### Projects

- `src/JobApplicationTracker.Api` – Presentation layer (controllers, contracts, middleware)
- `src/JobApplicationTracker.Application` – Application layer (CQRS handlers, validation)
- `src/JobApplicationTracker.Domain` – Domain entities and enums
- `src/JobApplicationTracker.Infrastructure` – EF Core persistence, repositories, logging
- `tests/*` – Unit and integration test projects mirroring the feature layout

### Local Development

```bash
dotnet build JobApplicationTracker.sln
dotnet ef database update --project src/JobApplicationTracker.Infrastructure --startup-project src/JobApplicationTracker.Api
dotnet run --project src/JobApplicationTracker.Api
```

Swagger UI is available at `/swagger`. API routes remain versioned under `/api/v1/job-applications`.

### Testing

```bash
dotnet test JobApplicationTracker.sln
```

### Docker

Build and run from the `apps/api` directory:

```bash
docker build -t job-application-tracker-api .
docker run --rm -p 8080:8080 job-application-tracker-api
```

The Dockerfile is colocated alongside this README.
