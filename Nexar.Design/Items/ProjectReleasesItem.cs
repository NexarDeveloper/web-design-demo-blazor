using MudBlazor;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class ProjectReleasesItem : TreeItem2
{
    public ProjectReleasesItem(ProjectItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly ProjectItem _parent;
    public override string Text => "Releases";
    public override string Icon => Icons.Filled.ContentPasteGo;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.ProjectReleases.ExecuteAsync(_parent.Tag.Id);
        res.EnsureNoErrors();

        return res.Data.DesProjectById.Design.Releases.Nodes
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (TreeItem)new ReleaseItem(x))
            .ToHashSet();
    }
}
