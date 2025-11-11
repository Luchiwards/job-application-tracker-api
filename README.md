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

## Docker

Build the production image:

```bash
docker build -t job-application-tracker-api .
```

Run the container, exposing port `8080` and persisting the SQLite database to your host:

```bash
docker run --rm -p 8080:8080 `
  -v ${PWD}/data:/app/data `
  job-application-tracker-api
```

- The API listens on `http://localhost:8080`.
- The SQLite database is stored under `/app/data/job-application-tracker.db`; bind a host directory if you want to persist data between runs.
- Override configuration via environment variables, e.g. `-e ConnectionStrings__Default="Data Source=/app/data/job-application-tracker.db"`.
