namespace LivePager.Gateway.Features.Mission.Responses
{
    public class GetMissionsResponse
    {
        public required GetMissionsResponseMissionDto[] Missions { get; init; }
    }

    public class GetMissionsResponseMissionDto
    {
        public required string Name { get; init; }
    }
}
