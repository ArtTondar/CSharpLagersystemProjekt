using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Layout;

public partial class MainLayout
{
    [Inject]
    public AuthState AuthState { get; set; } = default!;

    [Inject]
    public ILoginService LoginService { get; set; } = default!;

    private bool IsLoginModalOpen;
    protected override void OnInitialized()
    {
        AuthState.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AuthState.OnChange -= StateHasChanged;
    }
    protected override async Task OnInitializedAsync()
    {
        await AuthState.LoadCurrentUserAsync();
    }

    private void OpenLoginModal()
    {
        IsLoginModalOpen = true;
    }

    private Task CloseLoginModal()
    {
        IsLoginModalOpen = false;
        return Task.CompletedTask;
    }
}