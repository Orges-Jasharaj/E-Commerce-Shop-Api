using Blazored.LocalStorage;
using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace EcommerceUI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;


        public CustomAuthStateProvider(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("AuthApi");
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(identity));

        }
        public void NotifyUserAuthentication(string token)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Keep the existing public void for callers that expect a synchronous method.
        // It triggers the async logout logic without awaiting (fire-and-forget).
        public void NotifyUserLogout()
        {
            _ = NotifyUserLogoutAsync();
        }

        // Proper async logout method — clears tokens from local storage and notifies auth state change.
        public async Task NotifyUserLogoutAsync()
        {
            // Remove tokens from local storage
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");

            // Optionally clear any other stored auth-related keys here

            // Notify authentication state changed (anonymous user)
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var accessToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var client = _httpClientFactory.CreateClient("AuthApi");

            var request = new RefreshTokenRequestDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var response = await client.PostAsJsonAsync("refreshtoken", request);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<ResponseDto<RefreshTokenDto>>();

            if (result.Success)
            {
                await _localStorage.SetItemAsync("authToken", result.Data.AccessToken);
                await _localStorage.SetItemAsync("refreshToken", result.Data.RefreshToken);
            }
            NotifyUserAuthentication(result.Data.AccessToken);

            return result.Data.AccessToken;
        }


        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
