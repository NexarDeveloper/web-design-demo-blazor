using MudBlazor;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class ProjectTasksItem : TreeItem2
{
    public ProjectTasksItem(ProjectItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly ProjectItem _parent;
    public override string Text => "Tasks";
    public override string Icon => Icons.Material.Filled.Task;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.ProjectTasks.ExecuteAsync(_parent.Tag.Id);
        res.EnsureNoErrors();

        return res.Data.DesProjectTasks.Nodes
            .OrderByDescending(x => x.ModifiedAt)
            .Select(x => (TreeItem)new TaskItem(x, this))
            .ToHashSet();
    }
}
