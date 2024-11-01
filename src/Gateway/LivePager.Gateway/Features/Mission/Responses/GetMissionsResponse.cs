﻿using System.Runtime.Serialization;

namespace LivePager.Gateway.Features.Mission.Responses
{
    [DataContract]
    public class GetMissionsResponse
    {
        [DataMember]
        public required GetMissionsResponseMissionDto[] Missions { get; init; }
    }

    [DataContract]
    public class GetMissionsResponseMissionDto
    {
        [DataMember]
        public required string Name { get; init; }

        [DataMember]
        public string? Organization { get; init; }

        [DataMember]
        public required DateTime Created { get; set; }

        [DataMember]
        public DateTime? Updated { get; set; }
    }
}
