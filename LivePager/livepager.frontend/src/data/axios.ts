// axiosInstance.ts
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:32768/",
});

export default axiosInstance;
