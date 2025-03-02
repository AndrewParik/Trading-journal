import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5295/api', // Make sure this is your actual backend URL
  headers: {
    'Content-Type': 'application/json'
  }
});

export default api;
