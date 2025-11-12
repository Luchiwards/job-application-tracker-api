import './HomePage.css'

const HomePage = () => (
  <section className="home">
    <h1 className="home__title">Welcome to your application hub</h1>
    <p className="home__description">
      Track job opportunities, monitor progress, and collaborate with ease. Use the navigation to
      manage pipelines, review insights, or share updates with your team.
    </p>
    <div className="home__actions">
      <button type="button" className="home__primary-btn">
        Create new application
      </button>
      <button type="button" className="home__secondary-btn">
        View analytics
      </button>
    </div>
  </section>
)

export default HomePage
