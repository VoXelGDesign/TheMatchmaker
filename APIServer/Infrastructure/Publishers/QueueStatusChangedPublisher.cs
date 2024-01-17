using Contracts.QueueContracts;
using MassTransit;

namespace Infrastructure.Publishers
{
    public class QueueStatusChangedPublisher : IQueueStatusChangedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public QueueStatusChangedPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishAsync(QueueStatusChanged queueRequest)
        {
            await _publishEndpoint.Publish(queueRequest);
        }
    }
}
