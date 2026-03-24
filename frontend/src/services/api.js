// src/services/api.js
import axios from 'axios';

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('ctps_token');

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

api.interceptors.response.use(
    (response) => {
        if (response.data && response.data.success) {
            return response.data.data;
        }

        if (response.data && !response.data.success) {
            return Promise.reject(response.data.errors ? .[0] || 'An error occurred');
        }

        return response;
    },
    (error) => {
        const message =
            error.response ? .data ? .errors ? .[0] ||
            error.response ? .data ? .message ||
            error.message ||
            'Network Error';

        return Promise.reject(message);
    }
);

export default api;