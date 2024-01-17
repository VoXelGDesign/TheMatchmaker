using Contracts.QueueContracts;
using MassTransit;

namespace QueueService.Publishers.JoinedQueue;

public class JoinedQueuePublisher : IJoinedQueuePublisher
{
    private readonly IBus _publishEndpoint;

    public JoinedQueuePublisher(IBus publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    public async Task PublishAsync(UserJoinedQueue joinedQueueMessage)
    {
        await _publishEndpoint.Publish(joinedQueueMessage);
    }
}
