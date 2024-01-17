using MassTransit;
using Application.Interfaces;
using Contracts.QueueContracts.RocketLeague;

namespace Infrastructure.Publishers;
public class QueueRequestPublisher : IQueueRequestPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public QueueRequestPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishAsync(QueueRocketLeagueLobbyRequest queueRequest)
    {
        await _publishEndpoint.Publish(queueRequest);
    }
}

