namespace Contracts.ApiContracts.Lobby.RocketLeague;

public record RocketLeague2vs2LobbyResponse
{
    public RocketLeaguePlayerDto Player1 { get; set; }
    public RocketLeaguePlayerDto Player2 { get; set; }

    public RocketLeague2vs2LobbyResponse(RocketLeaguePlayerDto player1, RocketLeaguePlayerDto player2)
    {
        Player1 = player1;
        Player2 = player2; 
    }
}
