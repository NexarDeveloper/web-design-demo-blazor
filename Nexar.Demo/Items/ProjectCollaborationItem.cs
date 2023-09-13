using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectCollaborationItem : NodeTreeItem
{
    public ProjectCollaborationItem(ProjectItem parent) : base(parent)
    {
        Parent = parent;
    }

    public new ProjectItem Parent { get; }
    public override string Text => "Collaboration";
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new CollaborationDomainItem(this, DesCollaborationDomain.Ecad),
            new CollaborationDomainItem(this, DesCollaborationDomain.Mcad),
        });
    }
}
