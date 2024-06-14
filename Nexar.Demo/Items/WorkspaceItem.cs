using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceItem(IMyWorkspace tag, bool hasData) : TreeItem
{
    private readonly bool _hasData = hasData;
    public IMyWorkspace Tag { get; } = tag;
    public override string Text => Tag.Name;
    public override string Path => Tag.Name;
    public override bool CanExpand => _hasData;
    public override string Icon => _hasData ? Icons.Material.Filled.FolderOpen : Icons.Material.Filled.FolderOff;

    public override NexarClient Client =>
        NexarClientFactory.GetClient(AppData.IsRegionApi || AppData.ApiEndpoint.Contains("localhost") ? AppData.ApiEndpoint : Tag.Location.ApiServiceUrl);

    public override Task<HashSet<TreeItem>> ServerData()
    {
        HashSet<TreeItem> items;
        if (_hasData)
        {
            items =
            [
                new WorkspaceProjectsItem(this),
                new WorkspaceLibraryItem(this),
                new WorkspaceTasksItem(this),
                new WorkspaceUsersItem(this),
            ];
        }
        else
        {
            items = [];
        }
        return Task.FromResult(items);
    }

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Workspace";
    }

    public static event Action OnChange;
    public static WorkspaceItem Current { get; private set; }
}
