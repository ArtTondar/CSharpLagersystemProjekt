using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class LoginView
{
    private string Email { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;

    [Inject]
    public AuthState AuthState { get; set; } = default!;

    public string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

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

    private void ClearError()
    {
        ErrorMessage = string.Empty;
    }

    private void SetError(string message)
    {
        ErrorMessage = message;
    }
}