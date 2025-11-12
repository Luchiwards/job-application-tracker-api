import { NavLink, Outlet } from 'react-router-dom'

const MainLayout = () => (
  <div className="layout">
    <header className="layout__header">
      <NavLink className="layout__brand" to="/applications">
        Job Application Tracker
      </NavLink>
    </header>
    <main className="layout__content">
      <Outlet />
    </main>
    <footer className="layout__footer">
      <small>© {new Date().getFullYear()} Job Application Tracker by Luchiwards</small>
    </footer>
  </div>
)

export default MainLayout
