using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectReleasesItem(ProjectItem parent) : NodeTreeItem(parent)
{
    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Releases";
    public override string Icon => Icons.Material.Filled.Launch;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.ProjectReleases.ExecuteAsync(Parent.Tag.Id);
        res.AssertNoErrors();

        return res.Data.DesProjectById.Design.Releases.Nodes
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (TreeItem)new ReleaseItem(x, this))
            .ToHashSet();
    }
}
