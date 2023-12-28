using Contracts.QueueContracts;


namespace Application.Interfaces;
public interface IQueueRequestPublisher
{
    public Task PublishAsync(QueueRequest request);
}

