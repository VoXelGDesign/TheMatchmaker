using Contracts.QueueContracts.RocketLeague;

namespace Infrastructure.Publishers;

public interface IDeleteRocketLeagueQueueRequestPublisher
{
    public Task PublishAsync(DeleteRocketLeagueQueueRequestRequest request);
}
