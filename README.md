# Job Application Tracker API

Clean architecture ASP.NET Core Web API for managing job application data.

## Solution Structure

- `src/JobApplicationTracker.Api` – Presentation layer organised under `Features/*` (controllers + contracts) with shared helpers in `Common/`
- `src/JobApplicationTracker.Application` – Application layer (`Features/*` contains commands/queries/models, `Common/` hosts cross-cutting plumbing such as validation)
- `src/JobApplicationTracker.Domain` – Domain entities and value objects
- `src/JobApplicationTracker.Infrastructure` – Infrastructure services (`Features/*` hold repositories, `Persistence/` contains EF Core context/migrations)
- `tests/*` – Unit and integration test projects mirroring the feature layout

## Getting Started

```bash
dotnet restore
dotnet build
dotnet ef database update --project src/JobApplicationTracker.Infrastructure --startup-project src/JobApplicationTracker.Api
dotnet run --project src/JobApplicationTracker.Api
```

The API is versioned (`/api/v1/job-applications`) and Swagger UI is available at `/swagger` in development.

### Run Tests

```bash
dotnet test
```

## Environment Configuration

- Database connection string: `ConnectionStrings:Default`
- Database options: `Database:Provider`, `Database:EnableSensitiveLogging`
- Logging (Serilog): `Serilog` section in `appsettings.*.json`
- CORS origins: adjust in `ServiceCollectionExtensions.cs`

## Docker (Upcoming)

Containerization support is planned in a subsequent step. A production Dockerfile and compose stack will be added after finalizing the API surface.
