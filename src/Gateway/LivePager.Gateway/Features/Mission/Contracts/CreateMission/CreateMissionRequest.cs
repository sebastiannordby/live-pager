using System.Runtime.Serialization;

namespace LivePager.Gateway.Features.Mission.Contracts.CreateMission
{
    [DataContract]
    public sealed class CreateMissionRequest
    {
        [DataMember]
        public required string Name { get; set; }

        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public required decimal Longitude { get; set; }

        [DataMember]
        public required decimal Latitude { get; set; }

        [DataMember]
        public required decimal SearchRadius { get; set; }

        [DataMember]
        public string? Organization { get; set; }
    }
}
