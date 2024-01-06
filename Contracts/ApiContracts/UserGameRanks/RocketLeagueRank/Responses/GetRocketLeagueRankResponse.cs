using Contracts.QueueContracts.RocketLeague;

namespace Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
public record GetRocketLeagueRankResponse
{
    public RocketLeagueRankDto? Rank2vs2 { get; set; }
    public RocketLeagueRankDto? Rank3vs3 { get; set; }
}



