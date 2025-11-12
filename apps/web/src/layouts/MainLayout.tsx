import { NavLink, Outlet } from 'react-router-dom'

const MainLayout = () => (
  <div className="layout">
    <header className="layout__header">
      <NavLink className="layout__brand" to="/applications">
        Job Application Tracker
      </NavLink>
      <p className="layout__subtitle">
        Stay on top of every application opportunity with a centralized dashboard.
      </p>
      <nav className="layout__nav">
        <NavLink className="layout__nav-link" to="/applications" end>
          All Applications
        </NavLink>
        <NavLink className="layout__nav-link" to="/applications/new">
          Add Application
        </NavLink>
      </nav>
    </header>
    <main className="layout__content">
      <Outlet />
    </main>
    <footer className="layout__footer">
      <small>© {new Date().getFullYear()} Job Application Tracker</small>
    </footer>
  </div>
)

export default MainLayout
