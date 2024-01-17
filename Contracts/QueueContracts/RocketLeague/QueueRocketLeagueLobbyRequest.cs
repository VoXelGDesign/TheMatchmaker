using Contracts.Common;
using Contracts.QueueContracts.RocketLeague.Ranks;
using System.Data;

namespace Contracts.QueueContracts.RocketLeague;

public record QueueRocketLeagueLobbyRequest
{
    public UserIdDto UserId { get; set; }
    public RocketLeagueQueueMode Mode { get; set; }
    public QueueRocketLeagueRank UserRank { get; set; }
    public QueueRocketLeagueRank LowerBoundRank { get; set; }
    public QueueRocketLeagueRank UpperBoundRank { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow; 
}
