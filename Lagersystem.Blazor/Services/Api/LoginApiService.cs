using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Lagersystem.Blazor.Services.Api
{
    public class LoginApiService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task TryLoginAsync(string Email, string Password)
        {

            try
            {
                // Create request message
                var request = new HttpRequestMessage(HttpMethod.Post, "api/User/login")
                {
                    Content = JsonContent.Create(new
                    {
                        email = Email,
                        password = Password
                    })
                };

                // 🔑 Include credentials (cookies) in the browser request
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

                // Send request
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Login succeeded, cookie will be stored automatically
                    Console.WriteLine("Logged in ✅");
                }
                else
                {
                    Console.WriteLine("Login failed ❌");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
