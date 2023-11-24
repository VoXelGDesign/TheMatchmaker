using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Headers;

namespace Client
{
    public class BearerHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        public BearerHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenType = await _localStorageService.GetItemAsync<string>("TokenType");
            var accessToken = await _localStorageService.GetItemAsync<string>("AccessToken");

            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(tokenType))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
