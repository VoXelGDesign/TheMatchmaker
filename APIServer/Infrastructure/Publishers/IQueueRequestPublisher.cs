using Contracts.QueueContracts.RocketLeague;


namespace Application.Interfaces;
public interface IQueueRequestPublisher
{
    public Task PublishAsync(QueueRocketLeagueLobbyRequest request);
}

