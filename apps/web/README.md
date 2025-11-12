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

## Testing & Storybook (future)

Add testing/stories within `src/` and wire them to workspace-level scripts when the tooling is introduced (Vitest, Cypress, Storybook, etc.).

## Styling

Global tokens live in `src/styles/global.css`. Layout-specific styles belong inside the feature or layout folder (e.g. `pages/home/HomePage.css`).

## API Integration

Service modules under `src/services` should consume the backend via typed clients. Consider creating a shared `packages/contracts` package once API DTOs need to be reused across apps.
