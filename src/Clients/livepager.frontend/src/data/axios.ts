// axiosInstance.ts
import axios, { AxiosInstance } from "axios";

export default function getAxiousInstance(): AxiosInstance {
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
        window.location.href = "/authentication/login";
      }

      // Return the error to be handled by the calling function
      return Promise.reject(error);
    }
  );

  return axiosInstance;
}
