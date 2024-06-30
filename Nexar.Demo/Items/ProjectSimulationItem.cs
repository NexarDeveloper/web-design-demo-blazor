using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectSimulationItem(ProjectItem parent) : NodeTreeItem(parent)
{
    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Simulation";
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new SimulationDomainItem(this, "AnsysEDB"),
            new SimulationDomainItem(this, "PCBEDB"),
        });
    }
}
