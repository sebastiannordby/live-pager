import axiosInstance from "./axios";
import { LoginRequest, LoginResponse } from "./models/authentication/";
import {
  FindMissionResponse,
  CreateMissionRequest,
  FindMissionRequest,
  GetMissionsResponse,
} from "./models/mission";

async function login(request: LoginRequest): Promise<LoginResponse> {
  const response = await axiosInstance.post<LoginResponse>(
    "/api/authentication/login",
    request
  );

  return response.data as LoginResponse;
}

async function createMission(request: CreateMissionRequest): Promise<void> {
  await axiosInstance.post("/api/mission", request);
}

async function findMission(missionId: string): Promise<FindMissionResponse> {
  const response = await axiosInstance.get<FindMissionResponse>(
    `/api/mission/${missionId}`
  );

  return response.data as FindMissionResponse;
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
    findMission,
  },
};
