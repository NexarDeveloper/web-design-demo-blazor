using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SimulationDomainItem(ProjectSimulationItem parent, string domainName) : LeafTreeItem(parent)
{
    public string DomainName { get; private set; } = domainName;
    public IReadOnlyList<IMyCollaborationSimulationRevision>? Revisions { get; private set; }

    public new ProjectSimulationItem Parent { get; } = parent;
    public override string Text => DomainName;
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "SimulationDomain";
    }

    public static event Action? OnChange;
    public static SimulationDomainItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectSimulation.ExecuteAsync(Parent.Parent.Tag.Id, DomainName);
        var data = res.AssertNoErrors();

        Revisions = data.DesProjectCollaborationSimulationRevisions!.Nodes!.ToList();
    }
}
