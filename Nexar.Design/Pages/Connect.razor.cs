using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Nexar.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design.Pages
{
    public partial class Connect
    {
        [Inject] NexarClient Client { get; set; }

        string _token;
        bool _loading;

        /// <summary>
        /// Get token from the user input, then go to search.
        /// </summary>
        async Task ConnectAsync()
        {
            if (string.IsNullOrEmpty(_token))
                return;

            _loading = true;
            try
            {
                // share token for services
                AppData.Token = _token;

                // fetch workspaces
                var res = await Client.Workspaces.ExecuteAsync();
                EnsureNoErrors(res);

                // share workspaces
                AppData.SetWorkspaces(res.Data.DesWorkspaces);

                // save "good token"
                try
                {
                    await JS.InvokeVoidAsync("setLocalStorage", AppData.KeyToken, _token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// If a token is provided, check its sanity, then go to search.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // skip started
            _token = AppData.Token;
            if (!string.IsNullOrEmpty(_token))
                return;

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            _loading = true;
            try
            {
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("mode", out var mode))
                {
                    if (Enum.TryParse(mode, true, out AppMode value))
                        AppData.Mode = value;
                    else
                        throw new Exception($"Invalid mode.");
                }

                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out var token))
                {
                    _token = token;
                    AppData.Token = token;

                    var res = await Client.Workspaces.ExecuteAsync();
                    EnsureNoErrors(res);

                    AppData.SetWorkspaces(res.Data.DesWorkspaces);

                    // just to hide parameters in the address bar
                    NavManager.NavigateTo("");
                }
                else
                {
                    try
                    {
                        _token = await JS.InvokeAsync<string>("getLocalStorage", AppData.KeyToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync(ex.Message);
                NavManager.NavigateTo("");
            }
            finally
            {
                _loading = false;
            }
        }
    }
}
