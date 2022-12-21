using MudBlazor;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceProjectsItem : TreeItem2
{
    public WorkspaceProjectsItem(WorkspaceItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly WorkspaceItem _parent;
    public override string Text => "Projects";
    public override string Icon => Icons.Filled.Memory;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.Projects.ExecuteAsync(_parent.Tag.Url);
        res.EnsureNoErrors();

        return res.Data.DesProjects.Nodes
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new ProjectItem(x, this))
            .ToHashSet();
    }
}
