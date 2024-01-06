using Domain.Exceptions.CustomExceptions;

namespace Domain.Games.RocketLeague.Ranks;

public sealed record RocketLeagueRank
{
    public RocketLeagueRankName RocketLeagueRankName { get; private set; }

    public RocketLeagueRankNumber RocketLeagueRankNumber { get; private set; }

    public RocketLeagueDivision RocketLeagueDivision { get; private set; }


    public static RocketLeagueRank? Create(string name, string number, string division)
    {
        RocketLeagueRankName? parsedName = null;
        RocketLeagueRankNumber? parsedNumber = null;
        RocketLeagueDivision? parsedDivision = null;

        try
        {
            parsedName = (RocketLeagueRankName)Enum.Parse(
                typeof(RocketLeagueRankName),
                name);

            parsedNumber = (RocketLeagueRankNumber)Enum.Parse(
                typeof(RocketLeagueRankNumber),
                number);

            parsedDivision = (RocketLeagueDivision)Enum.Parse(
                typeof(RocketLeagueDivision),
                division);
        }
        catch 
        {
            return null;
        }

        if (parsedName is null || parsedNumber is null || parsedDivision is null) return null;

        var rlName = parsedName.Value;
        var rlNumber = parsedNumber.Value;
        var rlDivision = parsedDivision.Value;

        if (IsInvalidCombination(rlName, rlNumber, rlDivision)) return null;

        return new RocketLeagueRank
        {
            RocketLeagueRankName = rlName,
            RocketLeagueRankNumber = rlNumber,
            RocketLeagueDivision = rlDivision
        };
    }

    public static bool IsInvalidCombination(
        RocketLeagueRankName name,
        RocketLeagueRankNumber rankNumber,
        RocketLeagueDivision division)
        => (name == RocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber != RocketLeagueRankNumber.NONE || division != RocketLeagueDivision.NONE)) ||
            (name != RocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber == RocketLeagueRankNumber.NONE || division == RocketLeagueDivision.NONE));


}