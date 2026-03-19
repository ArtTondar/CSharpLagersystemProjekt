using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages
{
    public partial class LoginView
    {

        private string Email;
        private string Password;
        // OrderState bruges som mellemled mellem UI og service-lag.
        // Komponenten skal ikke selv kende til HttpClient eller API-endpoints.
        [Inject]
        public LoginState LoginState { get; set; } = default!;

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
                await LoginState.TryLoginAsync(Email, Password);
            }
            catch (Exception ex)
            {
                SetError($"Fejl ved hentning af valgt ordre: {ex.Message}");
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
