import axiosInstance from "./axios";
import { LoginRequest } from "./models/authentication/login-request";
import { LoginResponse } from "./models/authentication/login-response";

export async function login(request: LoginRequest): Promise<LoginResponse> {
  const response = await axiosInstance.post<LoginResponse>(
    "/api/authentication/login",
    request
  );

  return response.data as LoginResponse;
}

export const API = {
  authentication: {
    login,
  },
};
