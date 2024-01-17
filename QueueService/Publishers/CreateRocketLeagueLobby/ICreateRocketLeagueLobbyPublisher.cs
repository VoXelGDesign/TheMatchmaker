using Contracts.NotficiationContracts;
using Contracts.QueueContracts.RocketLeague;

namespace QueueService.Publishers.CreateRocketLeagueLobby;

public interface ICreateRocketLeagueLobbyPublisher
{
    public Task PublishAsync(CreateRocketLeagueLobbyRequest request);
}
