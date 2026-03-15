// src/services/api.js
import axios from 'axios';

const api = axios.create({
  // We will change this to Hirtik's actual localport later (e.g., https://localhost:7123/api)
  baseURL: 'http://localhost:5000/api', 
  headers: {
    'Content-Type': 'application/json',
  },
});

// Response Interceptor: Automatically unwraps Hirtik's CustomResult<T>
api.interceptors.response.use(
  (response) => {
    // If Hirtik's API returns success: true, extract the actual data payload
    if (response.data && response.data.success) {
      return response.data.data; 
    }
    // If success is false, reject it so your components can show the error message
    if (response.data && !response.data.success) {
      return Promise.reject(response.data.message || "An error occurred");
    }
    return response;
  },
  (error) => {
    // Handle standard HTTP errors (404, 500, etc.)
    const message = error.response?.data?.message || error.message || "Network Error";
    return Promise.reject(message);
  }
);

export default api;