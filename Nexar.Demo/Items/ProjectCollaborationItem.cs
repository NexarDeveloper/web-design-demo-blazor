using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class ProjectCollaborationItem(ProjectItem parent) : NodeTreeItem(parent)
{
    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Collaboration";
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new CollaborationDomainItem(this, DesCollaborationDomain.Ecad),
            new CollaborationDomainItem(this, DesCollaborationDomain.Mcad),
            new CollaborationDomainItem(this, DesCollaborationDomain.Esd),
        });
    }
}
