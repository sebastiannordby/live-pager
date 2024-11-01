export type CreateMissionRequest = {
  name: string;
  description: string | null;
  longitude: number;
  latitude: number;
  searchRadius: number;
};
