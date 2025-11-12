# Job Application Tracker API (`apps/api`)

Clean Architecture ASP.NET Core Web API powering the Job Application Tracker backend.

## Getting Started

1. **Install prerequisites**
   - [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download) (includes the `dotnet` CLI and EF Core tooling)
   - Optional: [Docker](https://www.docker.com/) if you prefer running the API in containers

2. **Restore dependencies**

   ```bash
   cd apps/api
   dotnet restore JobApplicationTracker.sln
   ```

3. **Apply database migrations**

   ```bash
   dotnet ef database update --project src/JobApplicationTracker.Infrastructure --startup-project src/JobApplicationTracker.Api
   ```

   The default configuration creates a SQLite database file named `job-application-tracker.db` under `src/JobApplicationTracker.Api`. Delete it to reset state or point the connection string at another provider.

4. **Run the API**

   ```bash
   dotnet run --project src/JobApplicationTracker.Api
   ```

   The development profile listens on `http://localhost:5199` with Swagger at `/swagger`. Containerised runs (e.g., `docker compose up api`) expose `http://localhost:8080` to match the web app defaults.

## Available Commands

All commands run from `apps/api`:

```bash
dotnet restore JobApplicationTracker.sln                        # Restore all solution dependencies
dotnet build JobApplicationTracker.sln                          # Compile projects and run analyzers
dotnet run --project src/JobApplicationTracker.Api              # Launch the API locally
dotnet watch run --project src/JobApplicationTracker.Api        # Launch with hot reload
dotnet ef database update --project ... --startup-project ...   # Apply the latest EF Core migrations
dotnet test JobApplicationTracker.sln                           # Execute unit, integration, and infrastructure tests
```

## Architecture Overview

- **Presentation (`src/JobApplicationTracker.Api`)** – ASP.NET Core controllers, middleware (exception handling, Serilog request logging), API versioning, health checks, and Swagger.
- **Application (`src/JobApplicationTracker.Application`)** – MediatR CQRS handlers, FluentValidation validators, and pipeline behaviours for cross-cutting rules.
- **Domain (`src/JobApplicationTracker.Domain`)** – Domain entities, enums, and guard clauses encapsulating business invariants.
- **Infrastructure (`src/JobApplicationTracker.Infrastructure`)** – EF Core DbContext, repositories, migrations, database options, Serilog configuration, and seeding utilities.
- **Tests (`tests/*`)** – Layer-aligned unit tests plus integration suites that boot the host via `ApiWebApplicationFactory`.

## API Endpoints

All routes are versioned under `/api/v1`:

- **List job applications**  
  `GET /api/v1/job-applications` flows through MediatR to a query handler, which requests a paginated result from the repository. The infrastructure layer translates the query to SQL and returns a `PaginatedList<JobApplicationDto>` with a 200 response.  
  ![List job applications sequence](documentation/list_applications.png)

- **Create job application**  
  `POST /api/v1/job-applications` wraps the payload in `CreateJobApplicationCommand`, validates it, persists the entity via EF Core, and returns a DTO with a `Location` header for the new identifier.  
  ![Create job application sequence](documentation/create_application.png)

- **Update job application**  
  `PUT /api/v1/job-applications/{id}` issues `UpdateJobApplicationCommand`, loads and updates the entity while guarding invariants, and returns 204 on success (or 404 if the record is missing).  
  ![Update job application sequence](documentation/update_application.png)

- **Delete job application**  
  `DELETE /api/v1/job-applications/{id}` triggers `DeleteJobApplicationCommand`, removes the record via the repository, and responds with 204 or propagates 404 when not found.  
  ![Delete job application sequence](documentation/delete_application.png)

Swagger (OpenAPI) documentation is served at `/swagger`.


## Environment Variables

The API uses ASP.NET Core configuration binding. Common overrides include:

- `ConnectionStrings__Default` – Override the SQLite connection string (e.g., point to SQL Server or PostgreSQL).
- `Database__Provider` – Choose the backing database provider (`sqlite` by default).
- `Database__EnableSensitiveLogging` – Enable EF Core sensitive data logging for local debugging.
- `ASPNETCORE_ENVIRONMENT` – Controls environment-specific configuration and logging (defaults to `Development`).
- `Serilog__MinimumLevel__Default` – Adjust the base logging level.

Set values via environment variables, `dotnet user-secrets`, or `appsettings.{Environment}.json`.



