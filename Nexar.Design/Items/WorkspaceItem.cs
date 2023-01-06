using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceItem : TreeItem
{
    public WorkspaceItem(IMyWorkspace tag)
    {
        Tag = tag;
    }

    public IMyWorkspace Tag { get; }
    public override string Text => Tag.Name;
    public override string Path => Tag.Name;
    public override string Icon => Icons.Material.Filled.FolderOpen;

    public override NexarClient Client =>
        NexarClientFactory.GetClient(AppData.ApiEndpoint.Contains("localhost") ? AppData.ApiEndpoint : Tag.Location.ApiServiceUrl);

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new WorkspaceProjectsItem(this),
            new WorkspaceTasksItem(this),
            new WorkspaceUsersItem(this),
        });
    }

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "workspace";
    }

    public static event Action OnChange;
    public static WorkspaceItem Current { get; private set; }
}
