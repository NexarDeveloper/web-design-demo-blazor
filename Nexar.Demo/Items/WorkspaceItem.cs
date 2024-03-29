﻿using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceItem(IMyWorkspace tag) : TreeItem
{
    public IMyWorkspace Tag { get; } = tag;
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
            new WorkspaceLibraryItem(this),
            new WorkspaceTasksItem(this),
            new WorkspaceUsersItem(this),
        });
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
