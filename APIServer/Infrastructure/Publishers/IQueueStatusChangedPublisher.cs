
using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;

namespace Infrastructure.Publishers;


public interface IQueueStatusChangedPublisher
{
    public Task PublishAsync(QueueStatusChanged request);
}