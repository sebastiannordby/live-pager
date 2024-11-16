using LivePager.Grains.Contracts;
using LivePager.Grains.Contracts.Mission;
using LivePager.Grains.Contracts.MissionParticipant;
using Microsoft.AspNetCore.SignalR;
using Orleans.Streams;

namespace LivePager.Gateway.Features.Mission
{
    public class MissionStreamHostedService : IHostedService
    {
        private readonly IClusterClient _clusterClient;
        private readonly IHubContext<MissionSignalRHub> _hubContext;
        private readonly ILogger<MissionStreamHostedService> _logger;
        private readonly Dictionary<Guid, StreamSubscriptionHandle<MissionLocationDataPoint>> _subscriptions;

        public MissionStreamHostedService(
            IClusterClient clusterClient,
            IHubContext<MissionSignalRHub> hubContext,
            ILogger<MissionStreamHostedService> logger)
        {
            _clusterClient = clusterClient;
            _hubContext = hubContext;
            _logger = logger;
            _subscriptions = new Dictionary<Guid, StreamSubscriptionHandle<MissionLocationDataPoint>>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            const int maxRetryAttempts = 5;
            const int delayMilliseconds = 1000;

            for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
            {
                try
                {
                    Console.WriteLine($"Attempting to access grain. Attempt {attempt} of {maxRetryAttempts}...");

                    var missionCollectionGrain = _clusterClient
                        .GetGrain<IMissionCollectionGrain>(LivePagerOrleansConstants.GlobalMissionCollection);

                    var missions = await missionCollectionGrain.GetMissionsAsync();
                    Console.WriteLine($"Successfully retrieved {missions.Length} missions.");

                    foreach (var mission in missions)
                    {
                        await SubscribeToMissionStream(mission.Id);
                    }

                    _logger.LogInformation("MissionStreamHostedService started.");

                    break; // Exit the retry loop on success
                }
                catch (Exception ex) when (attempt < maxRetryAttempts)
                {
                    Console.WriteLine($"Grain call failed: {ex.Message}. Retrying in {delayMilliseconds}ms...");
                    await Task.Delay(delayMilliseconds, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to access grain after {maxRetryAttempts} attempts: {ex.Message}");
                    throw; // Rethrow the exception if all retries fail
                }
            }

            _logger.LogInformation("MissionStreamHostedService started.");
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var handle in _subscriptions.Values)
            {
                await handle.UnsubscribeAsync();
            }

            _logger.LogInformation("MissionStreamHostedService stopped.");
        }

        public async Task SubscribeToMissionStream(Guid missionId)
        {
            if (_subscriptions.ContainsKey(missionId))
                return;

            var streamProvider = _clusterClient
                .GetStreamProvider(LivePagerOrleansConstants.DefaultStreamProvider);
            var streamId = StreamId
                .Create("participant_location", missionId);
            var stream = streamProvider
                .GetStream<MissionLocationDataPoint>(streamId);

            var handle = await stream.SubscribeAsync(async (locationDataPoint, token) =>
            {
                await _hubContext.Clients.Group(missionId.ToString())
                    .SendAsync("ReceiveLocationUpdate", locationDataPoint);
            });

            _subscriptions[missionId] = handle;

            _logger.LogInformation($"Subscribed to mission stream: {missionId}");
        }

        public async Task UnsubscribeFromMissionStream(Guid missionId)
        {
            if (_subscriptions.TryGetValue(missionId, out var handle))
            {
                await handle.UnsubscribeAsync();
                _subscriptions.Remove(missionId);

                _logger.LogInformation($"Unsubscribed from mission stream: {missionId}");
            }
        }
    }
}
