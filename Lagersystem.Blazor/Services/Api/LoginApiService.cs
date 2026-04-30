using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace Lagersystem.Blazor.Services.Api
{
    public class LoginApiService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrentUserDto?> TryLoginAsync(string email, string password)
        {
            HttpRequestMessage request = new(HttpMethod.Post, "api/User/login")
            {
                Content = JsonContent.Create(new
                {
                    email,
                    password
                })
            };

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CurrentUserDto>();
        }

        public async Task<CurrentUserDto?> GetCurrentUserAsync()
        {
            HttpRequestMessage request = new(HttpMethod.Get, "api/User/me");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CurrentUserDto>();
        }

        public async Task LogoutAsync()
        {
            HttpRequestMessage request = new(HttpMethod.Post, "api/User/logout");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}