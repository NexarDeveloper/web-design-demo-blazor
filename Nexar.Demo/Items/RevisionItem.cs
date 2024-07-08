using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class RevisionItem(IMyRelease tag, ProjectRevisionsItem parent) : LeafTreeItem(parent)
{
    public IMyRelease Tag { get; } = tag;
    public override string Text => Tag.CreatedAt.ToString();
    public override string Icon => Icons.Material.Filled.Launch;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "revision";
    }

    public static event Action? OnChange;
    public static RevisionItem? Current { get; private set; }
}
