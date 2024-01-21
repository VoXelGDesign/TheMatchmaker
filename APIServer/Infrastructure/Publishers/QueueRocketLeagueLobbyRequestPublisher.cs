using MassTransit;
using Application.Interfaces;
using Contracts.QueueContracts.RocketLeague;

namespace Infrastructure.Publishers;
public class QueueRocketLeagueLobbyRequestPublisher : IQueueRocketLeagueLobbyRequestPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public QueueRocketLeagueLobbyRequestPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishAsync(QueueRocketLeagueLobbyRequestDto queueRequest)
    {
        await _publishEndpoint.Publish(queueRequest);
    }
}

