import axios from 'axios'

const baseURL = import.meta.env.VITE_API_BASE_URL

if (!baseURL) {
  console.warn('VITE_API_BASE_URL is not defined. Set it in your .env file to enable API requests.')
}

const apiClient = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
})

export default apiClient

