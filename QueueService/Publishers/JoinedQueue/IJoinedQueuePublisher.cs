using Contracts.NotficiationContracts;
using Contracts.QueueContracts;

namespace QueueService.Publishers.JoinedQueue;

public interface IJoinedQueuePublisher
{
    public Task PublishAsync(UserJoinedQueue joinedQueueMessage);
}
