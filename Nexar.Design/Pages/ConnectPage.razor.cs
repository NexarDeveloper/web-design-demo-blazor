using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Design.Pages;

public partial class ConnectPage
{
    [Parameter]
    [SupplyParameterFromQuery(Name = "api")]
    public string ApiParameter { get; init; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "mode")]
    public string ModeParameter { get; init; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "token")]
    public string TokenParameter { get; init; }

    string _token;
    bool _loading;
    bool _connecting;

    static async Task SetTokenAndWorkspaces(string token)
    {
        // share token for services
        NexarClientFactory.AccessToken = token;

        // fetch workspaces
        var client = NexarClientFactory.GetClient(AppData.ApiEndpoint);
        var res = await client.Workspaces.ExecuteAsync();
        res.AssertNoErrors();

        // share workspaces
        AppData.SetWorkspaces(res.Data.DesWorkspaces);
    }

    /// <summary>
    /// Connect with the user input token.
    /// </summary>
    async Task ConnectAsync()
    {
        if (string.IsNullOrEmpty(_token))
            return;

        _connecting = true;
        try
        {
            // set top data
            await SetTokenAndWorkspaces(_token);

            // save "good token"
            try
            {
                await JS.InvokeVoidAsync("window.localStorage.setItem", AppData.KeyToken, _token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAndResetAsync(ex.Message);
        }
        finally
        {
            _connecting = false;
        }
    }

    /// <summary>
    /// If a token parameter is provided, connect right away.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // skip started
        _token = NexarClientFactory.AccessToken;
        if (!string.IsNullOrEmpty(_token))
            return;

        _loading = true;
        try
        {
            // default mode
            AppMode mode = AppMode.Prod;

            // mode parameter?
            if (!string.IsNullOrEmpty(ModeParameter))
            {
                if (Enum.TryParse(ModeParameter, true, out AppMode value))
                    mode = value;
                else
                    throw new Exception($"Invalid query parameter `mode`.");
            }

            // configure
            AppData.Initialize(mode, ApiParameter);

            // token parameter?
            if (!string.IsNullOrEmpty(TokenParameter))
            {
                // set top data
                _token = TokenParameter;
                await SetTokenAndWorkspaces(TokenParameter);

                // hide parameters in the address bar
                Navigation.NavigateTo("");
            }
            else
            {
                // the token is not provided, get it from the storage
                try
                {
                    _token = await JS.InvokeAsync<string>("window.localStorage.getItem", AppData.KeyToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            await ShowErrorAndResetAsync(ex.Message);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task PasteToken()
    {
        var (ok, value) = await JS.InvokeAsyncWithErrorHandling<string>("navigator.clipboard.readText");
        if (ok)
            _token = value;
    }
}
