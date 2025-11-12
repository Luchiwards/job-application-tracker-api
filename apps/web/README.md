# Web Application (`apps/web`)

React + TypeScript frontend bootstrapped with Vite and organised for a growing product surface.

## Getting Started

1. **Install prerequisites**
   - [Node.js 20+](https://nodejs.org/) (ships with npm 10+)
   - Running instance of the Job Application Tracker API (defaults to `http://localhost:8080`)

2. **Install dependencies**

   ```bash
   cd apps/web
   npm install
   ```

3. **Configure environment variables**

   ```bash
   cp .env.example .env
   ```

   Update `VITE_API_BASE_URL` so it points to your API (e.g. `http://localhost:8080/api`).

4. **Run the dev server**

   ```bash
   npm run dev
   ```

   The app starts on [http://localhost:3000](http://localhost:3000). Keep the API running so data loads correctly.

## Available Scripts

All commands are executed from `apps/web`:

```bash
npm run dev        # Start the Vite dev server with hot reload
npm run build      # Type-check and generate the production bundle
npm run preview    # Preview the production build locally
npm run lint       # Run ESLint across the project
npm run format     # Check formatting with Prettier
npm run format:fix # Automatically fix formatting issues
```

Repository-level npm scripts (`npm run dev:web`, etc.) proxy to the same commands if you prefer running them from the repo root.

## Architecture Overview

- **Routing (`src/App.tsx`)** – Uses React Router v6 with lazy-loaded pages. Routes are nested under `MainLayout` and include the applications list, create, edit, and not-found screens.
- **State Management (`src/store`)** – Redux Toolkit powers the global store. `jobApplicationsSlice.ts` uses an entity adapter for normalized caching, async thunks for network calls, and typed hooks (`useAppDispatch`, `useAppSelector`) for components.
- **UI Features (`src/pages/applications`)**
  - `ApplicationsListPage` fetches data, manages filters, and renders `JobApplicationsTable` with inline status updates and an overlay detail card.
  - `ApplicationCreatePage` and `ApplicationEditPage` reuse `ApplicationForm` for validated create/edit flows with optimistic feedback.
- **Services (`src/services`)** – `jobApplicationsApi.ts` wraps the Axios client and exposes typed helpers for list, read, create, update, change-status, and delete operations.
- **Shared Components** – `src/components` contains reusable pieces such as the form, table, status select, alerts, and pagination controls that keep UX consistent across pages.

## API Endpoints

The frontend talks to the Job Application Tracker API via the following REST endpoints (all relative to `VITE_API_BASE_URL`):

- `GET /job-applications` – List applications with pagination and filters.
- `GET /job-applications/:id` – Retrieve a single application.
- `POST /job-applications` – Create an application.
- `PUT /job-applications/:id` – Update an application or change its status.
- `DELETE /job-applications/:id` – Delete an application.

## Folder Layout

- `src/components` – Reusable UI pieces (forms, table, alerts, etc.)
- `src/hooks` – Typed Redux hooks
- `src/layouts` – Shared page shells (`MainLayout`)
- `src/pages` – Route-level screens
- `src/services` – API clients and network utilities
- `src/store` – Redux Toolkit store and slice definitions
- `src/styles` – Global and feature-level stylesheets
- `src/types`, `src/utils` – Shared types and helper utilities

Path aliases (see `tsconfig.app.json`) let you import with `@web/...`.

## Environment Variables

Vite exposes variables prefixed with `VITE_` at build-time:

- `VITE_API_BASE_URL` – Base URL for the REST API.
- `VITE_PAGE_SIZE` – Default page size for paginated queries.

Values in `.env` override `.env.example` locally and are ignored by git.
