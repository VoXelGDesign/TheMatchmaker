using Contracts.ApiContracts.Lobby.RocketLeague;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using MudBlazor;
using System.Net.Http.Json;

namespace Client.Lobby
{
    public class RocketLeagueLobbyManager : IRocketLeagueLobbyManager
    {
        private RocketLeague2vs2LobbyResponse? _response;

        private readonly HttpClient _httpClient;

        public RocketLeagueLobbyManager(IHttpClientFactory httpClientFactory, ISnackbar snackbar)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");   
        }

        public RocketLeague2vs2LobbyResponse? GetUserLobby()
            => _response;

        public async Task UpdateLobbyStatus()
        {
            var result =
                await _httpClient.GetAsync("/api/Lobby");

            if (!result.IsSuccessStatusCode)
                return;

            _response =
                await result.Content.ReadFromJsonAsync<RocketLeague2vs2LobbyResponse>();

        }
    }
}
