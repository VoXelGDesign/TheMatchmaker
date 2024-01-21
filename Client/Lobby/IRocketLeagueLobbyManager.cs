using Contracts.ApiContracts.Lobby.RocketLeague;

namespace Client.Lobby;

public interface IRocketLeagueLobbyManager
{
    public Task UpdateLobbyStatus();
    public RocketLeague2vs2LobbyResponse? GetUser2vs2Lobby();
    public RocketLeague3vs3LobbyResponse? GetUser3vs3Lobby();
}
