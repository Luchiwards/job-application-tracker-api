## Job Application Tracker API

Clean Architecture ASP.NET Core Web API hosted under `apps/api`.

### Projects

- `src/JobApplicationTracker.Api` – Presentation layer (controllers, contracts, middleware)
- `src/JobApplicationTracker.Application` – Application layer (CQRS handlers, validation)
- `src/JobApplicationTracker.Domain` – Domain entities and enums
- `src/JobApplicationTracker.Infrastructure` – EF Core persistence, repositories, logging
- `tests/*` – Unit and integration test projects mirroring the feature layout

### Endpoint Sequence Diagrams

- **List job applications**  
  The controller forwards `GET /api/v1/job-applications` to MediatR, which dispatches a query handler. The handler requests a paginated result from the repository, the infrastructure layer translates the request into SQL, and the resulting `PaginatedList<JobApplicationDto>` is returned to the client as a 200 response.  
  ![List job applications sequence](documentation/list_applications.png)

- **Create job application**  
  A `POST /api/v1/job-applications` request is wrapped in a `CreateJobApplicationCommand` and sent via MediatR. The command handler validates and instantiates the domain entity, persists it through the repository/EF Core, and returns a DTO and location header with the new identifier.  
  ![Create job application sequence](documentation/create_application.png)

- **Update job application**  
  The controller issues `UpdateJobApplicationCommand` for `PUT /api/v1/job-applications/{id}`. The handler loads the existing entity, applies updates while enforcing invariants, then persists the change. A successful update yields a 204 response; missing records surface as 404.  
  ![Update job application sequence](documentation/update_application.png)

- **Delete job application**  
  A `DeleteJobApplicationCommand` handles `DELETE /api/v1/job-applications/{id}`. The handler delegates to the repository to remove the record and returns 204 on success, propagating not-found cases as 404 to the client.  
  ![Delete job application sequence](documentation/delete_application.png)

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

The solution uses a layered test suite under `tests/` to follow the testing pyramid:

- **Application layer unit tests** keep the base of the pyramid broad by isolating handlers and validators. They mock repositories to prove business rules—e.g., `CreateJobApplicationCommandValidator` rejects future dates, and `UpdateJobApplicationCommandHandler` raises `NotFoundException` when a record is missing—so logic regressions surface quickly.
- **Infrastructure tests** run against an ephemeral SQLite instance to verify data-access code (`JobApplicationRepository`) without hitting production databases. They assert pagination and filtering behave as expected, catching issues that pure mocks would miss while staying fast.
- **API integration tests** boot the full host through `ApiWebApplicationFactory` and drive the HTTP CRUD flow end to end. This ensures routing, middleware, JSON contracts, and EF Core migrations work together, providing high-confidence coverage at the top of the pyramid without over-relying on slow tests.


