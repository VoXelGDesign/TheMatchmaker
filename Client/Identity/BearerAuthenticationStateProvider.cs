using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Json;
using Client.Identity.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;


namespace Client.Identity
{

    public class BearerAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        private readonly JsonSerializerOptions jsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

        private readonly HttpClient _httpClient;

        private readonly ILocalStorageService _localStorage;

        private bool _authenticated = false;

        private readonly ClaimsPrincipal Unauthenticated =
            new(new ClaimsIdentity());


        public BearerAuthenticationStateProvider(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient("Auth");
            _localStorage = localStorage;
        }

        public async Task<FormResult> RegisterAsync(string email, string password)
        {
            string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

            try
            {

                // make the request
                var result = await _httpClient.PostAsJsonAsync(
                    "register", new
                    {
                        email,
                        password
                    });

                // successful?
                if (result.IsSuccessStatusCode)
                {
                    return new FormResult { Succeeded = true };
                }

                // body should contain details about why it failed
                var details = await result.Content.ReadAsStringAsync();
                var problemDetails = JsonDocument.Parse(details);
                var errors = new List<string>();
                var errorList = problemDetails.RootElement.GetProperty("errors");

                foreach (var errorEntry in errorList.EnumerateObject())
                {
                    if (errorEntry.Value.ValueKind == JsonValueKind.String)
                    {
                        errors.Add(errorEntry.Value.GetString()!);
                    }
                    else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                    {
                        errors.AddRange(
                            errorEntry.Value.EnumerateArray().Select(
                                e => e.GetString() ?? string.Empty)
                            .Where(e => !string.IsNullOrEmpty(e)));
                    }
                }

                // return the error list
                return new FormResult
                {
                    Succeeded = false,
                    ErrorList = problemDetails == null ? defaultDetail : [.. errors]
                };
            }
            catch { }

            // unknown error
            return new FormResult
            {
                Succeeded = false,
                ErrorList = defaultDetail
            };
        }

        public async Task<FormResult> LoginAsync(string email, string password)
        {
            var result = await _httpClient.PostAsJsonAsync("login", new { email, password });

            if (!result.IsSuccessStatusCode)
            {
                return new FormResult { Succeeded = false, ErrorList = ["Invalid email and/or password."] };
            }

            var loginResponse = await result.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse == null)
            {
                return new FormResult { Succeeded = false, ErrorList = ["Invalid email and/or password."] };
            }

            await _localStorage.SetItemAsStringAsync("TokenType", loginResponse.TokenType);
            await _localStorage.SetItemAsStringAsync("AccessToken", loginResponse.AccessToken);
            await _localStorage.SetItemAsStringAsync("ExpiresIn", loginResponse.ExpiresIn.ToString());
            await _localStorage.SetItemAsStringAsync("RefreshToken", loginResponse.RefreshToken);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return new FormResult { Succeeded = true };

        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;

            // default to not authenticated
            var user = Unauthenticated;

            try
            {
                // the user info endpoint is secured, so if the user isn't logged in this will fail
                var userResponse = await _httpClient.GetAsync("manage/info");

                // throw if user info wasn't retrieved
                userResponse.EnsureSuccessStatusCode();

                // user is authenticated,so let's build their authenticated identity
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

                if (userInfo != null)
                {
                    // in our system name and email are the same
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userInfo.Email),
                        new(ClaimTypes.Email, userInfo.Email)
                    };

                    // add any additional claims
                    claims.AddRange(
                        userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                            .Select(c => new Claim(c.Key, c.Value)));

                    // set the principal
                    var id = new ClaimsIdentity(claims, nameof(BearerAuthenticationStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch { }

            // return the state
            return new AuthenticationState(user);
        }

        public async Task LogoutAsync()
        {
            // Remove the access token from storage
            await _localStorage.RemoveItemAsync("TokenType");
            await _localStorage.RemoveItemAsync("AccessToken");
            await _localStorage.RemoveItemAsync("ExpiresIn");
            await _localStorage.RemoveItemAsync("RefreshToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }
    }
}
