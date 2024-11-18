export type FindMissionResponse = {
  id: string;
  name: string;
  description: string | null;
  organization: string | null;
  longitude: number;
  latitude: number;
  searchRadius: number;
};
