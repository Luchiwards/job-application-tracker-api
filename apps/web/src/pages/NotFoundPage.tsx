import { Link } from 'react-router-dom'

/**
 * Displays a friendly message when the requested route does not exist.
 *
 * @returns {JSX.Element} Not-found page redirecting users to the applications list.
 */
const NotFoundPage = () => (
  <section className="not-found">
    <h1>Page not found</h1>
    <p>The page you are looking for doesn&apos;t exist or has been moved.</p>
    <Link className="not-found__link" to="/applications">
      Go back to applications
    </Link>
  </section>
)

export default NotFoundPage

