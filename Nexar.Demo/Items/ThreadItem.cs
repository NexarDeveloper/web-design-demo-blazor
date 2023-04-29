using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class ThreadItem : TreeItem3
{
    public ThreadItem(IMyThread tag, ProjectCommentsItem parent) : base(parent)
    {
        Tag = tag;
        _name = Tag.Comments.Count == 0 ? "<empty>" : Tag.Comments[0].Text.Trim();
    }

    readonly string _name;
    public IMyThread Tag { get; }
    public override string Text => _name;
    public override string Icon => Icons.Material.Filled.ChatBubbleOutline;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Thread";
    }

    public static event Action OnChange;
    public static ThreadItem Current { get; private set; }
}
