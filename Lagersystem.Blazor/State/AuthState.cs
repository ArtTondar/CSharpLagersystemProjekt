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

    // 👇 NYT
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public AuthState(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<bool> TryLoginAsync(string email, string password)
    {
        IsLoading = true;

        try
        {
            CurrentUserDto? user = await _loginService.TryLoginAsync(email, password);

            if (user == null)
            {
                return false;
            }

            CurrentUser = user;

            NotifyStateChanged(); // 👈 vigtigt

            return true;
        }
        finally
        {
            IsLoading = false;
        }
    }
    public async Task LoadCurrentUserAsync()
    {
        if (CurrentUser?.IsAuthenticated == true)
        {
            return;
        }

        IsLoading = true;

        try
        {
            CurrentUser = await _loginService.GetCurrentUserAsync();

            Console.WriteLine(CurrentUser is null
                ? "CurrentUser is null"
                : $"CurrentUser loaded: {CurrentUser.Name}, admin: {CurrentUser.IsAdmin}");
            NotifyStateChanged();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LogoutAsync()
    {
        await _loginService.LogoutAsync();
        CurrentUser = null;

        NotifyStateChanged(); // 👈 vigtigt
    }
}