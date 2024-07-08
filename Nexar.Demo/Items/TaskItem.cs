using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class TaskItem(IMyTask tag, NodeTreeItem parent) : LeafTreeItem(parent)
{
    public IReadOnlyList<IMyComment>? Comments { get; private set; }

    public IMyTask Tag { get; } = tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Task;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "Task";
    }

    public static event Action? OnChange;
    public static TaskItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.TaskComments.ExecuteAsync(Tag.Id);
        var data = res.AssertNoErrors();

        Comments = ((IMyTaskComments)data.Node!).Comments
            .OrderBy(x => x.CreatedAt)
            .ToList();
    }
}
