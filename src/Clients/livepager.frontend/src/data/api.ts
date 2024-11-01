import axiosInstance from "./axios";
import { LoginRequest } from "./models/authentication/login-request";
import { LoginResponse } from "./models/authentication/login-response";
import { CreateMissionRequest } from "./models/mission/create-mission-request";

export async function login(request: LoginRequest): Promise<LoginResponse> {
  const response = await axiosInstance.post<LoginResponse>(
    "/api/authentication/login",
    request
  );

  return response.data as LoginResponse;
}

export async function createMission(
  request: CreateMissionRequest
): Promise<void> {
  await axiosInstance.post<LoginResponse>("/api/mission", request);
}

export const API = {
  authentication: {
    login,
  },
  mission: {
    createMission,
  },
};
