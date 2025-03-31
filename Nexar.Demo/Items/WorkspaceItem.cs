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

    public override NexarClient Client => NexarClientFactory.GetClient(
        AppData.IsRegionApi || AppData.ApiEndpoint.Contains("localhost") || Tag.Location is null ?
        AppData.ApiEndpoint :
        Tag.Location.ApiServiceUrl);

    public List<TreeItem> CreateChildItems()
    {
        return [
            new WorkspaceProjectsItem(this),
            new WorkspaceLibraryItem(this),
            new WorkspaceTasksItem(this),
            new WorkspaceUsersItem(this),
            new WorkspaceWorkflowsItem(this),
            new WorkspaceConfigurationItem(this),
        ];
    }

    public override Task<List<TreeItem>> ServerData()
    {
        var items = _hasData ? CreateChildItems() : [];
        return Task.FromResult(items);
    }

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Workspace";
    }

    public static event Action? OnChange;
    public static WorkspaceItem? Current { get; private set; }
}
