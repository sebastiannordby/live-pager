export type GetMissionsResponse = {
  missions: GetMissionsResponseMissionDto[];
};

export type GetMissionsResponseMissionDto = {
  name: string;
  organization: string;
  created: Date;
  updated: Date;
};
