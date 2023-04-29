using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class TaskItem : TreeItem3
{
    public IReadOnlyList<IMyComment> Comments { get; private set; }

    public TaskItem(IMyTask tag, TreeItem2 parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyTask Tag { get; }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Task;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "Task";
    }

    public static event Action OnChange;
    public static TaskItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.TaskComments.ExecuteAsync(Tag.Id);
            res.AssertNoErrors();

            Comments = ((IMyTaskComments)res.Data.Node).Comments
                .OrderBy(x => x.CreatedAt)
                .ToList();
        }
        catch
        {
            Comments = Array.Empty<IMyComment>();
        }

        OnChange?.Invoke();
    }
}
