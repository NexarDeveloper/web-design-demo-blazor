using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nexar.Demo.Shared;

public partial class NavMenu : IDisposable
{
    [Inject]
    NavigationManager Navigation { get; init; } = null!;

    [Inject]
    private IDialogService DialogService { get; init; } = null!;

    protected override void OnInitialized()
    {
        AppData.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AppData.OnChange -= StateHasChanged;
        GC.SuppressFinalize(this);
    }

    async Task<IReadOnlyCollection<TreeItemData<TreeItem>>?> ServerData(TreeItem node)
    {
        try
        {
            var items = await node.ServerData();
            return items.Select(x => new MyTreeItemData(x)).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await DialogService.ShowMessageBox("Error", ex.Message);

            if (ex is HttpRequestException)
            {
                AppData.Reset();
                Navigation.NavigateTo("");
            }
        }
        return null;
    }

    void SelectedValueChanged(TreeItem node)
    {
        if (node is null)
            return;

        var page = node.SetCurrent();
        if (page is not null)
            Navigation.NavigateTo(page);
    }
}
