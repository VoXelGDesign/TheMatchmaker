using Contracts.QueueContracts;

namespace QueueService.Publishers.RemovedFromQueue
{
    public interface IRemovedFromQueuePublisher
    {
        public Task PublishAsync(UserRemovedFromQueue joinedQueueMessage);
    }

}
