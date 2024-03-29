﻿using Contracts.QueueContracts;
using Contracts.QueueContracts.RocketLeague;

namespace Contracts.ApiContracts.Queue.Requests;

public class QueueRocketLeagueRequest
{
    public string Mode { get; set; }
    public RocketLeagueRankDto LowerBound { get; set; }
    public RocketLeagueRankDto UpperBound { get; set; }
    public QueueRegion QueueRegion { get; set; }
    public RocketLeaguePlatform Platform { get; set; }
}
