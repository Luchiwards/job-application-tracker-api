import axios from 'axios'

// Read API base URL from Vite environment to keep client configuration flexible.
const baseURL = import.meta.env.VITE_API_BASE_URL

if (!baseURL) {
  console.warn('VITE_API_BASE_URL is not defined. Set it in your .env file to enable API requests.')
}

/**
 * Shared Axios instance configured for the job application tracker API.
 *
 * @returns {import('axios').AxiosInstance} Configured Axios client.
 */
const apiClient = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
})

export default apiClient

