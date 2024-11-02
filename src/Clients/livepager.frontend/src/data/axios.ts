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

axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // If the error response status is 401, redirect to the login page
    if (error.response && error.response.status === 401) {
      // Clear the token from localStorage
      localStorage.removeItem("token");

      // Redirect to the login page
      window.location.href = "/login"; // You can replace "/login" with your actual login route
    }

    // Return the error to be handled by the calling function
    return Promise.reject(error);
  }
);


export default axiosInstance;
