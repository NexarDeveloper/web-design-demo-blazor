using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceConfigurationItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    public IMyWorkspace Tag => Parent.Tag;
    public override string Text => "Configuration";
    public override string Icon => Icons.Material.Filled.Settings;
    public new WorkspaceItem Parent => (WorkspaceItem)base.Parent;

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new ProjectTemplatesItem(this),
        });
    }
}
