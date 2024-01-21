namespace Contracts.QueueContracts.RocketLeague.Ranks;

public sealed record QueueRocketLeagueRank
{
    public QueueRocketLeagueRankName RocketLeagueRankName { get; set; }

    public QueueRocketLeagueRankNumber RocketLeagueRankNumber { get; set; }

    public QueueRocketLeagueDivision RocketLeagueDivision { get; set; }


    public static QueueRocketLeagueRank? Create(QueueRocketLeagueRankDto queueRocketLeagueRankDto)
    {
        QueueRocketLeagueRankName? parsedName = null;
        QueueRocketLeagueRankNumber? parsedNumber = null;
        QueueRocketLeagueDivision? parsedDivision = null;

        try
        {
            parsedName = (QueueRocketLeagueRankName)Enum.Parse(
                typeof(QueueRocketLeagueRankName),
                queueRocketLeagueRankDto.RocketLeagueRankName);

            parsedNumber = (QueueRocketLeagueRankNumber)Enum.Parse(
                typeof(QueueRocketLeagueRankNumber),
                queueRocketLeagueRankDto.RocketLeagueRankNumber);

            parsedDivision = (QueueRocketLeagueDivision)Enum.Parse(
                typeof(QueueRocketLeagueDivision),
                queueRocketLeagueRankDto.RocketLeagueDivision);
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

    private static bool IsInvalidCombination(
        QueueRocketLeagueRankName name,
        QueueRocketLeagueRankNumber rankNumber,
        QueueRocketLeagueDivision division)
        => (name == QueueRocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber != QueueRocketLeagueRankNumber.NONE || division != QueueRocketLeagueDivision.NONE)) ||
            (name != QueueRocketLeagueRankName.SUPERSONICLEGEND &&
            (rankNumber == QueueRocketLeagueRankNumber.NONE || division == QueueRocketLeagueDivision.NONE));


}