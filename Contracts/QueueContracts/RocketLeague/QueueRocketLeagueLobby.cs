using Domain.Games.RocketLeague.Ranks;

namespace Contracts.QueueContracts.RocketLeague;

public record QueueRocketLeagueLobby
{
    public string UserId { get; set; }
    public string Mode { get; set; }
    public RocketLeagueRank UserRank { get; set; }
    public RocketLeagueRank LowerBoundRank { get; set; }
    public RocketLeagueRank UpperBoundRank { get; set; }
}
