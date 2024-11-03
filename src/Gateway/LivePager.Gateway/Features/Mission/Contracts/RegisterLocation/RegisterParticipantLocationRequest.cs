namespace LivePager.Gateway.Features.Mission.Contracts.RegisterLocation
{
    public sealed record RegisterParticipantLocationRequest
    {
        public Guid MissionId { get; set; }
        public Guid UserId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
