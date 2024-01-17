using Contracts.QueueContracts;
using MassTransit;

namespace QueueService.Publishers.RemovedFromQueue
{
    public class RemovedFromQueuePublisher : IRemovedFromQueuePublisher
    {
        private readonly IBus _publishEndpoint;

        public RemovedFromQueuePublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishAsync(UserRemovedFromQueue removedFromQueueMessage)
        {
            await _publishEndpoint.Publish(removedFromQueueMessage);
        }
    }
}
