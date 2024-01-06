using Contracts.QueueContracts.RocketLeague;

namespace Contracts.ApiContracts.Queue.Requests;

public class QueueRocketLeagueRequest
{
    public string Mode { get; set; }

    public RocketLeagueRankDto LowerBound { get; set; }

    public RocketLeagueRankDto UpperBound { get; set; }
}
