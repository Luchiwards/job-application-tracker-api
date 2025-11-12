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

### Frontend environment variables

Create a local env file for Vite before running the web app:

```bash
cd apps/web
cp .env.example .env.local   # or copy to .env if you prefer
```

Adjust `VITE_API_BASE_URL` if the API runs on a different host/port. `.env.local` stays out of git so you can keep machine-specific overrides, while `.env` can hold shared defaults.

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

- `npm run lint:web` – ESLint (flat config) against the React workspace
- `npm run build:web` – TypeScript build + production bundle
- `dotnet test apps/api/JobApplicationTracker.sln` – Unit and integration tests