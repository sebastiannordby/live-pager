import { FindMissionResponse } from "../../data/models/mission";

export function setActiveMission(mission: FindMissionResponse) {
  localStorage.setItem("active_mission", JSON.stringify(mission));
}

export function getActiveMission() {
  const activeMissionJson = localStorage.getItem("active_mission");
  const activeMission = activeMissionJson
    ? JSON.parse(activeMissionJson)
    : null;

  return activeMission;
}
