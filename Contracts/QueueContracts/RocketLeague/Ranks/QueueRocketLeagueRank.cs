namespace Contracts.QueueContracts.RocketLeague.Ranks;

public sealed record QueueRocketLeagueRank
{
    public QueueRocketLeagueRankName RocketLeagueRankName { get; private set; }

    public QueueRocketLeagueRankNumber RocketLeagueRankNumber { get; private set; }

    public QueueRocketLeagueDivision RocketLeagueDivision { get; private set; }


    public static QueueRocketLeagueRank? Create(string name, string number, string division)
    {
        QueueRocketLeagueRankName? parsedName = null;
        QueueRocketLeagueRankNumber? parsedNumber = null;
        QueueRocketLeagueDivision? parsedDivision = null;

        try
        {
            parsedName = (QueueRocketLeagueRankName)Enum.Parse(
                typeof(QueueRocketLeagueRankName),
                name);

            parsedNumber = (QueueRocketLeagueRankNumber)Enum.Parse(
                typeof(QueueRocketLeagueRankNumber),
                number);

            parsedDivision = (QueueRocketLeagueDivision)Enum.Parse(
                typeof(QueueRocketLeagueDivision),
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

        return new QueueRocketLeagueRank
        {
            RocketLeagueRankName = rlName,
            RocketLeagueRankNumber = rlNumber,
            RocketLeagueDivision = rlDivision
        };
    }

    public static bool IsInvalidCombination(
        QueueRocketLeagueRankName name,
        QueueRocketLeagueRankNumber rankNumber,
        QueueRocketLeagueDivision division)
        => (name == QueueRocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber != QueueRocketLeagueRankNumber.NONE || division != QueueRocketLeagueDivision.NONE)) ||
            (name != QueueRocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber == QueueRocketLeagueRankNumber.NONE || division == QueueRocketLeagueDivision.NONE));


}