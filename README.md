# Job Application Tracker API

A full-stack workspace that combines the existing Clean Architecture ASP.NET Core API with a React + TypeScript client built on Vite.

## Repository Layout

- `apps/api` – .NET solution (API, application, domain, infrastructure projects + test suites)
- `apps/web` – React + Vite frontend scaffolded with TypeScript and a scalable folder layout (contains all Node.js dependencies and tooling)

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Node.js ≥ 20](https://nodejs.org/)
- [Docker](https://www.docker.com/products/docker-desktop/)

## Install Dependencies

```bash
cd apps/web && npm install
dotnet restore apps/api/JobApplicationTracker.sln
```

### Apply EF Core migrations

Run the database migrations before starting the API so the SQLite file is up to date:

```bash
cd apps/api
dotnet ef database update --project src/JobApplicationTracker.Infrastructure --startup-project src/JobApplicationTracker.Api
```

### Frontend environment variables

Create a local env file for Vite before running the web app:

```bash
cd apps/web
cp .env.example .env   
```

Adjust `VITE_API_BASE_URL` if the API runs on a different host/port. `.env` stays out of git so you can keep machine-specific overridess.

## Docker Compose

To run the API and web containers together from the repository root:

```bash
docker compose up --build
```

- The API remains at `http://localhost:8080`.
- The web app is served from `http://localhost:3000`.
- API data is persisted under `./data/api` on the host.

Stop and remove containers with:

```bash
docker compose down
```



## Quality Checks

- `docker compose run --rm api-tests test JobApplicationTracker.sln` – Run .NET tests using the SDK toolbox container
- `docker compose run --rm web-tools install` – Install Node dependencies before running web tooling commands
- `docker compose run --rm web-tools run lint` – Lint the React workspace inside the Node toolbox container
- `docker compose run --rm web-tools run build` – Build the React app inside the Node toolbox container