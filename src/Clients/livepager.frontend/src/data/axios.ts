// axiosInstance.ts
import axios from "axios";

const jwtToken = localStorage.getItem("token");

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URI,
  headers: jwtToken
    ? {
        Authorization: `Bearer ${jwtToken.trim()}`,
      }
    : {},
});

export default axiosInstance;
