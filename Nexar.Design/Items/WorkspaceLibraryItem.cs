﻿using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceLibraryItem : TreeItem2
{
    public WorkspaceLibraryItem(WorkspaceItem parent) : base(parent)
    {
    }

    public new WorkspaceItem Parent => (WorkspaceItem)base.Parent;
    public IMyWorkspace Tag { get; }
    public override string Text => "Library";
    public override string Icon => Icons.Material.Filled.FolderOpen;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new ComponentTemplatesItem(this),
        });
    }
}
