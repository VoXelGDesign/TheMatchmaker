using MassTransit;
using Application.Interfaces;
using Infrastructure.Publishers.Contracts;

namespace Infrastructure.Publishers;
public class QueueRequestPublisher : IQueueRequestPublisher
{
    private readonly IPublishEndpoint _bus;

    public QueueRequestPublisher(IBus bus)
    {
        _bus = bus;
    }
    public async Task PublishAsync(QueueRequest queueRequest)
    {
       await _bus.Publish(queueRequest);
    }
}

