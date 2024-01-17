
using Domain.Games.RocketLeague.Ranks;

namespace Contracts.QueueContracts.RocketLeague;

public record RocketLeagueRankDto
{
    public string Name { get; set; }
    public string Number { get; set; }
    public string Division { get; set; }

    public RocketLeagueRankDto(string name, string number, string division)
    {
        Name = name;
        Number = number;
        Division = division;
    }

    public static RocketLeagueRankDto ParseRocketLeagueRankToDto(RocketLeagueRank rocketLeagueRank)
    {
        return new RocketLeagueRankDto(
            rocketLeagueRank.RocketLeagueRankName.ToString(),
            rocketLeagueRank.RocketLeagueRankNumber.ToString(),
            rocketLeagueRank.RocketLeagueDivision.ToString());
    }
};
