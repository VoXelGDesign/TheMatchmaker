
using Domain.Games.RocketLeague.Players;

namespace Domain.Games.RocketLeague.Lobbies;

public class RocketLeague3vs3Lobby
{
    public RocketLeague3vs3LobbyId Id { get; private set; } = null!;
    public RocketLeaguePlayer Player1 { get; private set; } = null!;
    public RocketLeaguePlayer Player2 { get; private set; } = null!;
    public RocketLeaguePlayer Player3 { get; private set; } = null!;

    public static RocketLeague3vs3Lobby Create(RocketLeaguePlayer player1, RocketLeaguePlayer player2, RocketLeaguePlayer player3)
    => new RocketLeague3vs3Lobby
    {
        Id = new RocketLeague3vs3LobbyId(Guid.NewGuid()),
        Player1 = player1,
        Player2 = player2,
        Player3 = player3
    };

    public bool AreAllPlayersReady()
        => Player1.IsReady && Player2.IsReady && Player3.IsReady;
}
