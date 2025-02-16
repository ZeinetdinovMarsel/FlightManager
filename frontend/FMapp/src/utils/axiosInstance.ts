import axios from "axios";
import { useAuthStore } from "../store/auth";

const API_URL = "http://localhost:5183/";

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
