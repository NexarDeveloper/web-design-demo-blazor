using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Design;

public sealed class ThreadItem : TreeItem3
{
    public ThreadItem(IMyThread tag)
    {
        Tag = tag;
        _name = Tag.Comments.Count == 0 ? "<empty>" : Tag.Comments[0].Text.Trim();
    }

    readonly string _name;
    public IMyThread Tag { get; }
    public override string Text => _name;
    public override string Icon => Icons.Filled.ChatBubbleOutline;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "thread";
    }

    public static event Action OnChange;
    public static ThreadItem Current { get; private set; }
}
