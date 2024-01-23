using Contracts.QueueContracts.RocketLeague;


namespace Infrastructure.Publishers;
public interface IQueueRocketLeagueLobbyRequestPublisher
{
    public Task PublishAsync(QueueRocketLeagueLobbyRequestDto request);
}

