using Blazored.LocalStorage;
using Client.MyAccount.Models;
using Client.Identity.Models;
using Radzen.Blazor.Rendering;
using System.Net.Http.Json;
using System.Security.Claims;
using MudBlazor;

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

        

        public async Task<UserAccountInfo?> GetUserAccountInfo()
        {
            var result = await _httpClient.GetAsync("api/UserAccount");
            var accountInfo = await result.Content.ReadFromJsonAsync<UserAccountInfo>();
            return accountInfo;
        }

        

        public async Task<UserAccountInfo?> UpdateUserAccountInfo(UserAccountInfo info)
        {
            var result = await _httpClient.PutAsJsonAsync("api/UserAccount", info);
            var accountInfo = await result.Content.ReadFromJsonAsync<UserAccountInfo>();
            return accountInfo;
        }

        public async Task<RocketLeagueRank?> GetRocketLeagueRank()
        {
            var result = await _httpClient.GetAsync("api/RocketLeagueRank");

            if (result.IsSuccessStatusCode)
            {
                var rank = await result.Content.ReadFromJsonAsync<RocketLeagueRank>();
                return rank;
            }
            else
            {
                _snackbar.Add("Cannot fetch rank!", MudBlazor.Severity.Warning);
            }
            
            return null;
        }
        public async Task<RocketLeagueRank?> UpdateRocketLeagueRank(RocketLeagueRank rank)
        {
            var result = await _httpClient.PutAsJsonAsync("api/RocketLeagueRank", rank);

            if (result.IsSuccessStatusCode)
            {
                var updatedRank = await result.Content.ReadFromJsonAsync<RocketLeagueRank>();
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
