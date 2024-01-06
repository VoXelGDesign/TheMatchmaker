using System.Net.Http.Json;
using MudBlazor;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
using Contracts.ApiContracts.UserAccountInfo.Requests;

namespace Client.MyAccount
{
    public class MyAccountManager : IMyAccount
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public MyAccountManager(IHttpClientFactory httpClientFactory, ISnackbar snackbar)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _snackbar = snackbar;
        }
  
        public async Task<GetUserAccountInfoResponse?> GetUserAccountInfo()
        {
            var result = await _httpClient.GetAsync("api/UserAccount");
            var accountInfo = await result.Content.ReadFromJsonAsync<GetUserAccountInfoResponse>();
            return accountInfo;
        }

        
        public async Task<UpdateUserAccountInfoResponse?> UpdateUserAccountInfo(UpdateUserAccountInfoRequest info)
        {
            var result = await _httpClient.PutAsJsonAsync("api/UserAccount", info);
            var accountInfo = await result.Content.ReadFromJsonAsync<UpdateUserAccountInfoResponse>();
            return accountInfo;
        }

        public async Task<GetRocketLeagueRankResponse?> GetRocketLeagueRank()
        {
            var result = await _httpClient.GetAsync("api/RocketLeagueRank");

            if (result.IsSuccessStatusCode)
            {
                var rank = await result.Content.ReadFromJsonAsync<GetRocketLeagueRankResponse>();
                return rank;
            }
            else
            {
                _snackbar.Add("Cannot fetch rank!", MudBlazor.Severity.Warning);
            }
            
            return null;
        }

        public async Task<UpdateRocketLeagueRankResponse?> UpdateRocketLeagueRank(UpdateRocketLeagueRankRequest rank)
        {
            var result = await _httpClient.PutAsJsonAsync("api/RocketLeagueRank", rank);

            if (result.IsSuccessStatusCode)
            {
                var updatedRank = await result.Content.ReadFromJsonAsync<UpdateRocketLeagueRankResponse>();
                _snackbar.Add("Rank was updated!", MudBlazor.Severity.Success);
                return updatedRank;
            }
            else
            {
                _snackbar.Add("Rank was not updated!", MudBlazor.Severity.Error);
            }
          
            return null;
        }


    }
}
