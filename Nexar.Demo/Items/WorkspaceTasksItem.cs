using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceTasksItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    readonly WorkspaceItem _parent = parent;
    public override string Text => "Tasks";
    public override string Icon => Icons.Material.Filled.Task;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.WorkspaceTasks.ExecuteAsync(_parent.Tag.Url);
        var data = res.AssertNoErrors();

        return data.DesWorkspaceTasks!.Nodes!
            .OrderByDescending(x => x.ModifiedAt)
            .Select(x => (TreeItem)new TaskItem(x, this))
            .ToList();
    }
}
