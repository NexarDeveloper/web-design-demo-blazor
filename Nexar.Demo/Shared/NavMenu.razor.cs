﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo.Shared;

public partial class NavMenu : IDisposable
{
    [Inject] NavigationManager NavManager { get; init; }

    protected override void OnInitialized()
    {
        AppData.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AppData.OnChange -= StateHasChanged;
        GC.SuppressFinalize(this);
    }

    static async Task<HashSet<TreeItem>> ServerData(TreeItem node)
    {
        try
        {
            var items = await node.ServerData();
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return null;
    }

    void SelectedValueChanged(TreeItem node)
    {
        if (node is null)
            return;

        var page = node.SetCurrent();
        if (page is not null)
            NavManager.NavigateTo(page);
    }
}
