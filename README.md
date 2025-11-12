# Job Application Tracker API

A full-stack workspace that combines the existing Clean Architecture ASP.NET Core API with a React + TypeScript client built on Vite.

## Repository Layout

- `apps/api` – .NET solution (API, application, domain, infrastructure projects + test suites)
- `apps/web` – React + Vite frontend scaffolded with TypeScript and a scalable folder layout (contains all Node.js dependencies and tooling)

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Node.js ≥ 20](https://nodejs.org/)

## Install Dependencies

```bash
cd apps/web && npm install
dotnet restore apps/api/JobApplicationTracker.sln
```

## Running Applications

- **API**

  ```bash
  dotnet build apps/api/JobApplicationTracker.sln
  dotnet ef database update --project apps/api/src/JobApplicationTracker.Infrastructure --startup-project apps/api/src/JobApplicationTracker.Api
  dotnet run --project apps/api/src/JobApplicationTracker.Api
  ```

  Swagger UI is available at `/swagger` in development. The HTTP surface remains versioned under `/api/v1/job-applications`.

- **Frontend**
  ```bash
  npm run dev:web
  ```
  Or run directly from `apps/web`:
  ```bash
  cd apps/web && npm run dev
  ```
  The development server defaults to `http://localhost:5173`.

## Quality Checks

- `npm run lint:web` – ESLint (flat config) against the React workspace
- `npm run build:web` – TypeScript build + production bundle
- `dotnet test apps/api/JobApplicationTracker.sln` – Unit and integration tests

## Conventions

- TypeScript path aliases are declared in `apps/web/tsconfig.base.json`:
  - `@web/*` resolves to `apps/web/src/*`
  - `@datacom/*` reserved for future shared packages under `packages/*/src`
- Frontend formatting rules live in `apps/web/prettier.config.mjs`; editor defaults are in `.editorconfig`.
- Align backend changes with the Clean Architecture layout (`Features`, `Common`, `Persistence`, etc.) preserved under `apps/api/src`.

## Docker (API)

The API Dockerfile moved to `apps/api/Dockerfile`. Build and run from the repo root:

```bash
docker build -t job-application-tracker-api ./apps/api
docker run --rm -p 8080:8080 `
  -v ${PWD}/data:/app/data `
  job-application-tracker-api
```

- The API listens on `http://localhost:8080`.
- The SQLite database now lives under `apps/api/src/JobApplicationTracker.Api/job-application-tracker.db`. Bind a host directory if you need persistence.
- Override configuration via environment variables, e.g. `-e ConnectionStrings__Default="Data Source=/app/data/job-application-tracker.db"`.
