using Contracts.QueueContracts.RocketLeague;


namespace Application.Interfaces;
public interface IQueueRocketLeagueLobbyRequestPublisher
{
    public Task PublishAsync(QueueRocketLeagueLobbyRequestDto request);
}

