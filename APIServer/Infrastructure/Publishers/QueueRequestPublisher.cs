using MassTransit;
using Application.Interfaces;
using Contracts.QueueContracts.RocketLeague;

namespace Infrastructure.Publishers;
public class QueueRequestPublisher : IQueueRequestPublisher
{
    private readonly IPublishEndpoint _bus;

    public QueueRequestPublisher(IPublishEndpoint bus)
    {
        _bus = bus;
    }
    public async Task PublishAsync(QueueRocketLeagueLobby queueRequest)
    {
       await _bus.Publish(queueRequest);
    }
}

