import axiosInstance from "./axios";
import { LoginRequest } from "./models/authentication/login-request";
import { LoginResponse } from "./models/authentication/login-response";
import { CreateMissionRequest } from "./models/mission/create-mission-request";
import { GetMissionsResponse } from "./models/mission/get-missions-response";

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
  await axiosInstance.post("/api/mission", request);
}

export async function getMissions(): Promise<GetMissionsResponse> {
  const response = await axiosInstance.get<GetMissionsResponse>("/api/mission");

  return response.data as GetMissionsResponse;
}

export const API = {
  authentication: {
    login,
  },
  mission: {
    getMissions,
    createMission,
  },
};
