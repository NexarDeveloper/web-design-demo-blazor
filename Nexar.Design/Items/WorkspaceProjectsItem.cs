using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceProjectsItem : TreeItem2
{
    public WorkspaceProjectsItem(WorkspaceItem parent) : base(parent)
    {
        Parent = parent;
    }

    public new WorkspaceItem Parent { get; }
    public override string Text => "Projects";
    public override string Icon => Icons.Material.Filled.Memory;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.Projects.ExecuteAsync(Parent.Tag.Url);
        res.AssertNoErrors();

        return res.Data.DesProjects.Nodes
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new ProjectItem(x, this))
            .ToHashSet();
    }
}
