using Contracts.ApiContracts.Lobby.RocketLeague;

namespace Client.Lobby;

public interface IRocketLeagueLobbyManager
{
    public Task UpdateLobbyStatus();
    public RocketLeague2vs2LobbyResponse? GetUserLobby();
}
