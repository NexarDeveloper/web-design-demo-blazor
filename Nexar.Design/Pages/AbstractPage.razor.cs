using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Nexar.Design.Pages;

/// <summary>
/// Common base page with handy members.
/// </summary>
public class AbstractPage : ComponentBase, IDisposable
{
    /// <summary>
    /// Common JS interop.
    /// </summary>
    [Inject]
    public IJSRuntime JS { get; init; }

    /// <summary>
    /// Common navigation manager.
    /// </summary>
    [Inject]
    public NavigationManager Navigation { get; init; }

    /// <summary>
    /// Message boxes.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; init; }

    /// <summary>
    /// Show the message, logout, navigate to login page.
    /// </summary>
    public async Task ShowErrorAndResetAsync(string message)
    {
        await DialogService.ShowMessageBox("Error", message);

        AppData.Reset();
        Navigation.NavigateTo("");
    }

    public void Dispose()
    {
        Disposing();
        GC.SuppressFinalize(this);
    }

    protected virtual void Disposing()
    {
    }
}
