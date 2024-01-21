using Contracts.ApiContracts.Lobby.RocketLeague;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using MudBlazor;
using System.Net.Http.Json;

namespace Client.Lobby
{
    public class RocketLeagueLobbyManager : IRocketLeagueLobbyManager
    {
        private RocketLeague2vs2LobbyResponse? _response2vs2;

        private RocketLeague3vs3LobbyResponse? _response3vs3;

        private readonly HttpClient _httpClient;

        public RocketLeagueLobbyManager(IHttpClientFactory httpClientFactory, ISnackbar snackbar)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");   
        }

        public RocketLeague2vs2LobbyResponse? GetUser2vs2Lobby()
            => _response2vs2;

        public RocketLeague3vs3LobbyResponse? GetUser3vs3Lobby()
            => _response3vs3;

        public async Task UpdateLobbyStatus()
        {
            var result2vs2 =
                await _httpClient.GetAsync("/api/Lobby/2vs2");

            var result3vs3 =
                await _httpClient.GetAsync("/api/Lobby/3vs3");

            if (result2vs2.IsSuccessStatusCode)
            {              
                _response2vs2 =
                await result2vs2.Content.ReadFromJsonAsync<RocketLeague2vs2LobbyResponse>();
                return;
            }

            _response2vs2 = null;

            if (result3vs3.IsSuccessStatusCode)
            {      
                _response3vs3 =
                await result3vs3.Content.ReadFromJsonAsync<RocketLeague3vs3LobbyResponse>();
                return;
            }

            _response3vs3 = null;

        }

        
    }
}
