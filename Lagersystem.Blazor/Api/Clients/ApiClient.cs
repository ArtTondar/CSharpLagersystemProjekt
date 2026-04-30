using System.Net.Http.Json;

namespace Lagersystem.Blazor.API.Clients;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        // Sender et GET request til API'et
        // og forsøger at læse JSON-svaret som typen T.
        return await _httpClient.GetFromJsonAsync<T>(url);
    }

    public async Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest request)
    {
        // Sender et POST request med request-objektet som JSON.
        return await _httpClient.PostAsJsonAsync(url, request);
    }

    public async Task<HttpResponseMessage> PutAsync<TRequest>(string url, TRequest request)
    {
        // Sender et PUT request med request-objektet som JSON.
        return await _httpClient.PutAsJsonAsync(url, request);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        // Sender et DELETE request til API'et.
        return await _httpClient.DeleteAsync(url);
    }
}