using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectTasksItem(ProjectItem parent) : NodeTreeItem(parent)
{
    readonly ProjectItem _parent = parent;
    public override string Text => "Tasks";
    public override string Icon => Icons.Material.Filled.Task;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.ProjectTasks.ExecuteAsync(_parent.Tag.Id);
        res.AssertNoErrors();

        return res.Data.DesProjectTasks.Nodes
            .OrderByDescending(x => x.ModifiedAt)
            .Select(x => (TreeItem)new TaskItem(x, this))
            .ToList();
    }
}
