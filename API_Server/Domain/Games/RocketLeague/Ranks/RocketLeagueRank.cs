namespace Domain.Games.RocketLeague.Ranks;

public sealed record RocketLeagueRank
{
    public RocketLeagueRankName RocketLeagueRankName { get; private set; }

    public RocketLeagueRankNumber RocketLeagueRankNumber { get; private set; }

    public RocketLeagueDivision RocketLeagueDivision { get; private set; }


    public static RocketLeagueRank? Create(
        RocketLeagueRankName name,
        RocketLeagueRankNumber rankNumber,
        RocketLeagueDivision division)
    {

        if (IsInvalidCombination(name, rankNumber, division)) return null;

        return new RocketLeagueRank
        {
            RocketLeagueRankName = name,
            RocketLeagueRankNumber = rankNumber,
            RocketLeagueDivision = division
        };
    }

    public static bool IsInvalidCombination(
        RocketLeagueRankName name,
        RocketLeagueRankNumber rankNumber,
        RocketLeagueDivision division)
        => name == RocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber != RocketLeagueRankNumber.NONE || division != RocketLeagueDivision.NONE);

}