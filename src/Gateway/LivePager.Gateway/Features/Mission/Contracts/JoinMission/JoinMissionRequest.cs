namespace LivePager.Gateway.Features.Mission.Contracts.JoinMission
{
    public sealed record JoinMissionRequest
    {
        public Guid MissionId { get; set; }
        public Guid UserId { get; set; }
    }
}
