namespace Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
public record UpdateRocketLeagueRankRequest
{
    public string Mode { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string Division { get; set; }

    public UpdateRocketLeagueRankRequest(string mode, string name, string number, string division)
    {
        Mode = mode;
        Name = name;
        Number = number;
        Division = division;
    }

}

