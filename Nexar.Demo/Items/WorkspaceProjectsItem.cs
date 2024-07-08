using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceProjectsItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    public new WorkspaceItem Parent { get; } = parent;
    public override string Text => "Projects";
    public override string Icon => Icons.Material.Filled.Memory;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.Projects.ExecuteAsync(Parent.Tag.Url);
        var data = res.AssertNoErrors();

        return data.DesProjects!.Nodes!
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new ProjectItem(x, this))
            .ToList();
    }
}
