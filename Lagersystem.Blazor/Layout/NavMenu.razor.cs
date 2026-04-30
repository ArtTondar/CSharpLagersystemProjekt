using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Layout;

public partial class NavMenu : ComponentBase, IDisposable
{
    [Inject]
    public AuthState AuthState { get; set; } = default!;

    private bool collapseNavMenu = true;

    private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    protected override void OnInitialized()
    {
        AuthState.OnChange += HandleAuthStateChanged;
    }

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
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