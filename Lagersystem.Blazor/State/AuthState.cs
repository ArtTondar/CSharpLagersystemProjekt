using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class AuthState
{
    private readonly ILoginService _loginService;

    public CurrentUserDto? CurrentUser { get; private set; }

    public bool IsLoading { get; private set; }

    public bool IsLoggedIn => CurrentUser?.IsAuthenticated == true;

    public bool IsAdmin => CurrentUser?.IsAdmin == true;

    public string DisplayName => CurrentUser?.Name ?? "Ikke logget ind";

    public string Role => CurrentUser?.Role ?? "Ukendt";

    public event Action? OnChange;

    public AuthState(ILoginService loginService)
    {
        _loginService = loginService;
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }

    public async Task<bool> TryLoginAsync(string email, string password)
    {
        IsLoading = true;

        try
        {
            CurrentUserDto? user = await _loginService.TryLoginAsync(email, password);

            if (user is null)
            {
                return false;
            }

            CurrentUser = user;
            NotifyStateChanged();
            return true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadCurrentUserAsync()
    {
        IsLoading = true;

        try
        {
            CurrentUser = await _loginService.GetCurrentUserAsync();
        }
        finally
        {
            IsLoading = false;
            NotifyStateChanged();
        }
    }

    public async Task LogoutAsync()
    {
        await _loginService.LogoutAsync();
        CurrentUser = null;
        NotifyStateChanged();
    }
}