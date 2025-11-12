import { configureStore } from '@reduxjs/toolkit'

import jobApplicationsReducer from './jobApplicationsSlice'

export const store = configureStore({
  reducer: {
    jobApplications: jobApplicationsReducer,
  },
  devTools: import.meta.env.DEV,
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

