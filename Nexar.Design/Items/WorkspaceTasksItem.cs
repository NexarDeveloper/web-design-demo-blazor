﻿using MudBlazor;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceTasksItem : TreeItem2
{
    public WorkspaceTasksItem(WorkspaceItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly WorkspaceItem _parent;
    public override string Text => "Tasks";
    public override string Icon => Icons.Filled.Task;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.WorkspaceTasks.ExecuteAsync(_parent.Tag.Url);
        res.EnsureNoErrors();

        return res.Data.DesWorkspaceTasks.Nodes
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new TaskItem(x, this))
            .ToHashSet();
    }
}
