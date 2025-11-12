# Web Application (`apps/web`)

React + TypeScript frontend bootstrapped with Vite and organised for a growing product surface.

## Folder Layout

- `src/components` – Reusable UI pieces
- `src/hooks` – Custom React hooks
- `src/layouts` – Shared page shells (root layout, auth layout, etc.)
- `src/pages` – Route-level screens (`home`, `pipeline`, `settings`, …)
- `src/services` – API clients and network utilities
- `src/store` – Global state management (Redux Toolkit, Zustand, etc.)
- `src/styles` – Global + feature styles
- `src/types`, `src/utils` – Cross-cutting types and helpers

Path aliases (see `tsconfig.app.json`) let you import with `@web/...`.

## Scripts

Run these commands from the `apps/web` directory:

```bash
npm run dev        # Start the Vite dev server
npm run build      # Type check + production bundle
npm run lint       # ESLint using the shared config
npm run format     # Check code formatting
npm run format:fix # Fix code formatting
npm run preview    # Preview production build
```

Or use the convenience scripts from the repository root:

```bash
npm run dev:web    # Start the Vite dev server
npm run build:web  # Type check + production bundle
npm run lint:web   # ESLint
```

## Environment Variables

Runtime configuration lives in `.env` files (Vite exposes variables that start with `VITE_`). Copy the template and adjust values for your environment:

```bash
cp .env.example .env.local   # or copy to .env if you want shared defaults
```

- `VITE_API_BASE_URL` – Base URL for the API (e.g. `http://localhost:8080/api/v1`)
- `VITE_PAGE_SIZE` – Default page size for the job applications table

Vite merges `.env` (ignored by git) with the checked-in defaults when you run `npm run dev`, so it’s ideal for machine-specific values.

## Testing & Storybook (future)

Add testing/stories within `src/` and wire them to workspace-level scripts when the tooling is introduced (Vitest, Cypress, Storybook, etc.).

## Job Applications Module

- Dedicated routes power the CRUD flow:
  - `/applications` – Paginated list with status filtering and in-table status updates
  - `/applications/new` – Form to add a new application with client-side validation
  - `/applications/:id/edit` – Edit existing records with real-time feedback
- State is managed with Redux Toolkit entity adapters for normalized caching and async thunks for API calls.
- All backend calls flow through the typed Axios client in `src/services`.

## Styling

Global tokens live in `src/styles/global.css`. Layout-specific styles belong inside the feature or layout folder (e.g. `pages/home/HomePage.css`).

Shared visual primitives (alerts, tables, forms, pagination) are defined in `src/styles/app.css` to keep the application UI consistent.

## API Integration

Service modules under `src/services` should consume the backend via typed clients. Consider creating a shared `packages/contracts` package once API DTOs need to be reused across apps.
