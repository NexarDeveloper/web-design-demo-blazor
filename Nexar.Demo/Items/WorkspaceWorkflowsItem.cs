using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class WorkspaceWorkflowsItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    readonly WorkspaceItem _parent = parent;
    public override string Text => "Workflows";
    public override string Icon => Icons.Material.Filled.AccountTree;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.WorkspaceWorkflows.ExecuteAsync(_parent.Tag.WorkspaceId);
        var data = res.AssertNoErrors();

        var workflows = data?.DesWorkspaceById?.Workflows;
        if (workflows is null)
            return [];

        return workflows
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new WorkflowItem(x, this))
            .ToList();
    }
}
