using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Nexar.Design.Pages
{
    public partial class Connect
    {
        [Inject]
        NexarClient Client { get; set; }

        Validations validations;
        string _token;
        bool _loading;

        static void ValidateNotEmpty(ValidatorEventArgs e)
        {
            var text = Convert.ToString(e.Value);
            e.Status =
                string.IsNullOrWhiteSpace(text) ? ValidationStatus.Error : ValidationStatus.Success;
        }

        static void ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                if (jwtSecurityToken.ValidTo < DateTime.UtcNow.AddSeconds(10))
                    throw new Exception("Expired");
            }
            catch (Exception)
            {
                throw new Exception("Invalid or expired token.");
            }
        }

        async Task AcceptToken(string token)
        {
            ValidateToken(token);
            AppData.Token = token;
            await JS.InvokeVoidAsync("setLocalStorage", AppData.KeyToken, token);
        }

        /// <summary>
        /// Get token from the user input, then go to search.
        /// </summary>
        async Task ConnectAsync()
        {
            if (!validations.ValidateAll())
                return;

            _loading = true;
            try
            {
                await AcceptToken(_token.Trim());

                var res = await Client.Workspaces.ExecuteAsync();
                EnsureNoErrors(res);

                AppData.SetWorkspaces(res.Data.DesWorkspaces);
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
            if (_token != null)
                return;

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
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
                    ValidateToken(token);
                    _token = token;
                    AppData.Token = token;

                    _loading = true;
                    try
                    {
                        var res = await Client.Workspaces.ExecuteAsync();
                        EnsureNoErrors(res);

                        AppData.SetWorkspaces(res.Data.DesWorkspaces);

                        // just to hide parameters in the address bar
                        NavManager.NavigateTo("");
                    }
                    finally
                    {
                        _loading = false;
                    }
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
                return;
            }
        }
    }
}
