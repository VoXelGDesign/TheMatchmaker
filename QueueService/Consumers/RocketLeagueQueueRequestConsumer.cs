using Contracts.QueueContracts.RocketLeague;
using MassTransit;

namespace QueueService.Consumers;

public class RocketLeagueQueueRequestConsumer : IConsumer<QueueRocketLeagueLobbyRequestDto>
{
    private readonly IServiceProvider _serviceProvider;
    public RocketLeagueQueueRequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<QueueRocketLeagueLobbyRequestDto> context)
    {
        var request = context.Message;

        var rocketLeagueQueue = _serviceProvider
            .GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();

        var queueEntry = QueueRocketLeagueLobbyRequest.CreateFromDto(request);

        if (rocketLeagueQueue is null || request is null || queueEntry is null)
            return;

        await rocketLeagueQueue.AddToQueue(queueEntry);
    }
}
