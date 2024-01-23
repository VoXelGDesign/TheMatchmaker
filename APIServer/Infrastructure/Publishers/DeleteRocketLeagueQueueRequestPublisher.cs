using Contracts.QueueContracts.RocketLeague;
using MassTransit;

namespace Infrastructure.Publishers;

public class DeleteRocketLeagueQueueRequestPublisher : IDeleteRocketLeagueQueueRequestPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteRocketLeagueQueueRequestPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishAsync(DeleteRocketLeagueQueueRequestRequest request)
    {
        await _publishEndpoint.Publish(request);
    }
}
