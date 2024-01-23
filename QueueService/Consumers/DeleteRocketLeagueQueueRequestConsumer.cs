using Contracts.QueueContracts.RocketLeague;
using MassTransit;

namespace QueueService.Consumers;

public class DeleteRocketLeagueQueueRequestConsumer : IConsumer<DeleteRocketLeagueQueueRequestRequest>
{
    private readonly IServiceProvider _serviceProvider;
    public DeleteRocketLeagueQueueRequestConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<DeleteRocketLeagueQueueRequestRequest> context)
    {
        var request = context.Message.UserIdDto;

        var rocketLeagueQueue = _serviceProvider
            .GetServices<IHostedService>()
            .OfType<RocketLeagueQueue>()
            .SingleOrDefault();
       
        if (rocketLeagueQueue is null || request.UserId is null)
            return;

        await rocketLeagueQueue.RemoveFromQueue(request);
    }
}