import type { ReactNode } from 'react'

const MainLayout = ({ children }: { children: ReactNode }) => (
  <div className="layout">
    <header className="layout__header">
      <a className="layout__brand" href="/">
        Job Application Tracker
      </a>
      <p className="layout__subtitle">
        Stay on top of every application opportunity with a centralized dashboard.
      </p>
    </header>
    <main className="layout__content">{children}</main>
    <footer className="layout__footer">
      <small>© {new Date().getFullYear()} Job Application Tracker</small>
    </footer>
  </div>
)

export default MainLayout
