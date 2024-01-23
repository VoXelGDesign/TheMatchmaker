using Contracts.QueueContracts;

namespace Infrastructure.Publishers;

public interface IQueueStatusChangedPublisher
{
    public Task PublishAsync(QueueStatusChanged request);
}