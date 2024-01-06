using Domain.Games.RocketLeague.Ranks;

namespace Contracts.QueueContracts.RocketLeague;

public record RocketLeagueRankDto(string Name, string Number, string Division)
{
    public static RocketLeagueRankDto ParseRocketLeagueRankToDto(RocketLeagueRank rocketLeagueRank)
    {
        return new RocketLeagueRankDto(
            rocketLeagueRank.RocketLeagueRankName.ToString(),
            rocketLeagueRank.RocketLeagueRankNumber.ToString(),
            rocketLeagueRank.RocketLeagueDivision.ToString());
    }
};
