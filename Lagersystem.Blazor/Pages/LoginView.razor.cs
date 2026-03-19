using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages
{
    public partial class LoginView
    {

        private string Email { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;
        // OrderState bruges som mellemled mellem UI og service-lag.
        // Komponenten skal ikke selv kende til HttpClient eller API-endpoints.
        [Inject]
        public AuthState AuthState { get; set; } = default!;

        // Fejltekst hvis API-kald fejler.
        public string ErrorMessage { get; set; } = string.Empty;

        // Bruges til at styre om fejlbeskeden skal vises.
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);



        // Henter den konkrete ordre igen ud fra dens id.
        // Det er nyttigt hvis man vil vise detaljer eller senere lave edit-view.
        public async Task TryLoginAsync()
        {
            ClearError();

            try
            {
                bool success = await AuthState.TryLoginAsync(Email, Password);

                if (!success)
                {
                    SetError("Login mislykkedes.");
                }
            }
            catch (Exception ex)
            {
                SetError($"Fejl under login: {ex.Message}");
            }
        }
        // Nulstiller tidligere fejl før et nyt API-kald.
        private void ClearError()
        {
            ErrorMessage = string.Empty;
        }

        // Sætter fejlbesked hvis noget går galt.
        private void SetError(string message)
        {
            ErrorMessage = message;
        }
    }
}
