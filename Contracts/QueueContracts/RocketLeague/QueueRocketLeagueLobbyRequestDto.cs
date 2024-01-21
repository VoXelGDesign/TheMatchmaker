using Contracts.Common;
using Contracts.QueueContracts.RocketLeague.Ranks;

namespace Contracts.QueueContracts.RocketLeague;

public record QueueRocketLeagueLobbyRequestDto
{
    public UserIdDto UserId { get; set; }
    public RocketLeagueQueueMode Mode { get; set; }
    public QueueRocketLeagueRankDto UserRank { get; set; }
    public QueueRocketLeagueRankDto LowerBoundRank { get; set; }
    public QueueRocketLeagueRankDto UpperBoundRank { get; set; }
    public QueueRegion Region { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow; 
}
