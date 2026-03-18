using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Lagersystem.Blazor.Shared;

public partial class LoginModal : ComponentBase
{
    [Inject]
    public AuthState AuthState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    private string Email { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;
    private string Message { get; set; } = string.Empty;
    private bool IsError { get; set; }
    private bool IsLoading { get; set; }

    private async Task HandleLoginSubmitAsync()
    {
        await TryLoginAsync();
    }

    private async Task TryLoginAsync()
    {
        Message = string.Empty;
        IsError = false;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            IsError = true;
            Message = "Email og password skal udfyldes.";
            return;
        }

        IsLoading = true;

        try
        {
            bool success = await AuthState.TryLoginAsync(Email, Password);

            if (!success)
            {
                IsError = true;
                Message = "Login mislykkedes. Tjek email og password.";
                return;
            }

            Message = $"Login lykkedes. Rolle: {AuthState.Role}";
        }
        catch
        {
            IsError = true;
            Message = "Der opstod en fejl under login.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LogoutAsync()
    {
        await AuthState.LogoutAsync();

        Email = string.Empty;
        Password = string.Empty;
        Message = string.Empty;
        IsError = false;

        await OnClose.InvokeAsync();
    }

    private async Task Close()
    {
        await OnClose.InvokeAsync();
    }
}