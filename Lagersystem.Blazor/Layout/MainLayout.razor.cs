using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Layout;

public partial class MainLayout : IDisposable
{
    [Inject]
    public AuthState AuthState { get; set; } = default!;

    [Inject]
    public ILoginService LoginService { get; set; } = default!;

    private bool IsLoginModalOpen;

    protected override void OnInitialized()
    {
        AuthState.OnChange += HandleAuthStateChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        await AuthState.LoadCurrentUserAsync();
    }

    private void OpenLoginModal()
    {
        IsLoginModalOpen = true;
        StateHasChanged();
    }

    private Task CloseLoginModal()
    {
        IsLoginModalOpen = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void HandleAuthStateChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        AuthState.OnChange -= HandleAuthStateChanged;
    }
}