using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net;
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
            var request = new HttpRequestMessage(HttpMethod.Post, "api/User/login")
            {
                Content = JsonContent.Create(new
                {
                    email,
                    password
                })
            };

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<CurrentUserDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<CurrentUserDto?> GetCurrentUserAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/User/me");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<CurrentUserDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/User/logout");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            try
            {
                await _httpClient.SendAsync(request);
            }
            catch
            {
                // Logout should fail silently in UI service layer.
            }
        }
    }
}