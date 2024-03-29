﻿using Domain.Games.RocketLeague.Players;

namespace Domain.Games.RocketLeague.Lobbies;
public class RocketLeague2vs2Lobby
{
    public RocketLeague2vs2LobbyId Id { get; private set; } = null!;
    public RocketLeaguePlayer Player1 { get; private set; } = null!;
    public RocketLeaguePlayer Player2 { get; private set; } = null!;
    public DateTime CreationDate { get; private set; }

    public static RocketLeague2vs2Lobby Create(RocketLeaguePlayer player1, RocketLeaguePlayer player2)
    => new RocketLeague2vs2Lobby
    {
        Id = new RocketLeague2vs2LobbyId(Guid.NewGuid()),
        Player1 = player1,
        Player2 = player2,
        CreationDate = DateTime.UtcNow
    };

    public bool AreAllPlayersReady()
        => Player1.IsReady && Player2.IsReady;
}
