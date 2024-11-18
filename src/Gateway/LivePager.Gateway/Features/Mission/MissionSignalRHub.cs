using Microsoft.AspNetCore.SignalR;

namespace LivePager.Gateway.Features.Mission
{
    public class MissionSignalRHub : Hub
    {
        public async Task JoinMissionGroup(string missionId)
        {
            // Add the client to the group associated with the mission
            await Groups.AddToGroupAsync(Context.ConnectionId, missionId);
        }

        public async Task LeaveMissionGroup(string missionId)
        {
            // Remove the client from the mission group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, missionId);
        }
    }
}
