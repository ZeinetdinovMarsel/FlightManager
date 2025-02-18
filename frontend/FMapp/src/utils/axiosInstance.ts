import axios from "axios";
import { useAuthStore } from "../services/authService";

export const API_URL = "https://localhost:5183/";
const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
    "Authorization":""
  },
});


axiosInstance.interceptors.request.use(
  (config) => {
    const token = useAuthStore.getState().refreshToken;
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);



export default axiosInstance;
