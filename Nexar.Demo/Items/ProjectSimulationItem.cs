using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectSimulationItem : NodeTreeItem
{
    public ProjectSimulationItem(ProjectItem parent) : base(parent)
    {
        Parent = parent;
    }

    public new ProjectItem Parent { get; }
    public override string Text => "Simulation";
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new SimulationDomainItem(this, "AnsysEDB"),
            new SimulationDomainItem(this, "PCBEDB"),
        });
    }
}
