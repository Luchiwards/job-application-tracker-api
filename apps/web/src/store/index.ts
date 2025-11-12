import { configureStore } from '@reduxjs/toolkit'

import jobApplicationsReducer from './jobApplicationsSlice'

/**
 * Redux store wiring job application slice with devtools toggle.
 */
export const store = configureStore({
  reducer: {
    jobApplications: jobApplicationsReducer,
  },
  devTools: import.meta.env.DEV,
})

/**
 * Root state shape inferred from the configured store.
 */
export type RootState = ReturnType<typeof store.getState>
/**
 * Dispatch type for typed hooks and thunks.
 */
export type AppDispatch = typeof store.dispatch

