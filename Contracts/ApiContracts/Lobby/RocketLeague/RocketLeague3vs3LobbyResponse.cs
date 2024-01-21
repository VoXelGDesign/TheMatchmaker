namespace Contracts.ApiContracts.Lobby.RocketLeague;

public record RocketLeague3vs3LobbyResponse
{

    public RocketLeaguePlayerDto Player1 { get; set; }
    public RocketLeaguePlayerDto Player2 { get; set; }
    public RocketLeaguePlayerDto Player3 { get; set; }
    public DateTime CreationDate { get; set; }

    public RocketLeague3vs3LobbyResponse(RocketLeaguePlayerDto player1, RocketLeaguePlayerDto player2, RocketLeaguePlayerDto player3, DateTime creationDate)
    {
        Player1 = player1;
        Player2 = player2;
        Player3 = player3;
        CreationDate = creationDate;
    }

}
