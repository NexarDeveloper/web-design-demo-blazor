using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SimulationDomainItem : LeafTreeItem
{
    public string DomainName { get; private set; }
    public IReadOnlyList<IMyCollaborationSimulationRevision> Revisions { get; private set; }

    public SimulationDomainItem(ProjectSimulationItem parent, string domainName) : base(parent)
    {
        DomainName = domainName;
        Parent = parent;
    }

    public new ProjectSimulationItem Parent { get; }
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

    public static event Action OnChange;
    public static SimulationDomainItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectSimulation.ExecuteAsync(Parent.Parent.Tag.Id, DomainName);
        res.AssertNoErrors();

        Revisions = res.Data.DesProjectCollaborationSimulationRevisions.Nodes.Reverse().ToList();
    }
}
