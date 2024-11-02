export type GetMissionsResponse = {
  missions: GetMissionsResponseMissionDto[];
};

export type GetMissionsResponseMissionDto = {
  id: string;
  name: string;
  organization: string;
  created: Date;
  updated: Date;
};
