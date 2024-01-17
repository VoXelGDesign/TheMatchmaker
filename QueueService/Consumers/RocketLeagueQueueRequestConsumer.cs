using Contracts.QueueContracts.RocketLeague;
using MassTransit;

namespace QueueService.Consumers;

public class RocketLeagueQueueRequestConsumer : IConsumer<QueueRocketLeagueLobbyRequest>
{
    private readonly IServiceProvider _serviceProvider;
    public RocketLeagueQueueRequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<QueueRocketLeagueLobbyRequest> context)
    {
        var request = context.Message;

        var rocketLeagueQueue = _serviceProvider
            .GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();

        if (rocketLeagueQueue is not null && request is not null)
            await rocketLeagueQueue.AddToQueue(request);

    }
}
