namespace Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
public record UpdateRocketLeagueRankResponse
{
    public string Name { get; set; }
    public string Number { get; set; }
    public string Division { get; set; }

    public UpdateRocketLeagueRankResponse(string name, string number, string division)
    {
        Name = name;
        Number = number;
        Division = division;
    }
}

