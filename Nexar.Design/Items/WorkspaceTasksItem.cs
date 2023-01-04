using MudBlazor;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class WorkspaceTasksItem : TreeItem2
{
    public WorkspaceTasksItem(WorkspaceItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly WorkspaceItem _parent;
    public override string Text => "Tasks";
    public override string Icon => Icons.Material.Filled.Task;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.WorkspaceTasks.ExecuteAsync(_parent.Tag.Url);
        res.EnsureNoErrors();

        return res.Data.DesWorkspaceTasks.Nodes
            .OrderByDescending(x => x.ModifiedAt)
            .Select(x => (TreeItem)new TaskItem(x, this))
            .ToHashSet();
    }
}
