using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Design;

public sealed class TaskItem : TreeItem3
{
    public TaskItem(IMyTask tag)
    {
        Tag = tag;
    }

    public IMyTask Tag { get; }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Filled.Task;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "task";
    }

    public static event Action OnChange;
    public static TaskItem Current { get; private set; }
}
