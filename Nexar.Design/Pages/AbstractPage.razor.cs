using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StrawberryShake;
using System;
using System.Threading.Tasks;

namespace Nexar.Design.Pages;

/// <summary>
/// Common base page with handy members.
/// </summary>
public partial class AbstractPage : ComponentBase
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
    /// Show the message, logout, navigate to login page.
    /// </summary>
    public async Task ShowErrorAsync(string message)
    {
        await JS.InvokeVoidAsync("alert", message);

        AppData.Reset();
        Navigation.NavigateTo("");
    }

    /// <summary>
    /// Gets line count for a text field.
    /// </summary>
    public static int CountLines(string text)
    {
        int n = 1;
        foreach (var c in text)
            if (c == '\n')
                ++n;
        return n;
    }
}
