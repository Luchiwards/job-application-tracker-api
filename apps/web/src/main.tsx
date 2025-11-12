import { StrictMode } from 'react'
import { Provider } from 'react-redux'
import { BrowserRouter } from 'react-router-dom'
import { createRoot } from 'react-dom/client'
import './styles/global.css'
import App from './App'
import { store } from './store'

// Bootstraps the React tree with global providers and client-side routing.
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Provider>
  </StrictMode>,
)
