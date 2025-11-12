import { Suspense, lazy } from 'react'
import { Navigate, Route, Routes } from 'react-router-dom'
import MainLayout from './layouts/MainLayout'
import './styles/app.css'

const ApplicationsListPage = lazy(() => import('./pages/applications/ApplicationsListPage'))
const ApplicationCreatePage = lazy(() => import('./pages/applications/ApplicationCreatePage'))
const ApplicationEditPage = lazy(() => import('./pages/applications/ApplicationEditPage'))
const NotFoundPage = lazy(() => import('./pages/NotFoundPage'))

const App = () => (
  <Suspense fallback={<div className="app__loading">Loading...</div>}>
    <Routes>
      <Route element={<MainLayout />}>
        <Route index element={<Navigate to="/applications" replace />} />
        <Route path="applications">
          <Route index element={<ApplicationsListPage />} />
          <Route path="new" element={<ApplicationCreatePage />} />
          <Route path=":id/edit" element={<ApplicationEditPage />} />
        </Route>
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  </Suspense>
)

export default App
