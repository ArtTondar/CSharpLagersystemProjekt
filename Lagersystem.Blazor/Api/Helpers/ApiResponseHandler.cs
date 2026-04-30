namespace Lagersystem.Blazor.API.Helpers;

public static class ApiResponseHandler
{
    public static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        // Hvis requestet lykkedes, skal vi ikke gøre mere.
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        // Hvis requestet fejlede, prøver vi at læse fejlteksten
        // fra API'et, så fejlen bliver lettere at forstå.
        string errorMessage = await TryReadErrorMessageAsync(response);

        throw new HttpRequestException(
            $"API request failed. Status: {(int)response.StatusCode}. Message: {errorMessage}");
    }

    private static async Task<string> TryReadErrorMessageAsync(HttpResponseMessage response)
    {
        // Hvis API'et ikke returnerer noget indhold,
        // giver vi en standardtekst tilbage.
        if (response.Content == null)
        {
            return "No error content returned from API.";
        }

        string content = await response.Content.ReadAsStringAsync();

        // Hvis indholdet er tomt eller kun whitespace,
        // bruger vi også en standardtekst.
        if (string.IsNullOrWhiteSpace(content))
        {
            return "No error content returned from API.";
        }

        return content;
    }
}