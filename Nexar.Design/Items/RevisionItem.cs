using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Design;

public sealed class RevisionItem : TreeItem3
{
    public RevisionItem(IMyRelease tag, ProjectRevisionsItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyRelease Tag;
    public override string Text => Tag.CreatedAt.ToString();
    public override string Icon => Icons.Material.Filled.Launch;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "revision";
    }

    public static event Action OnChange;
    public static RevisionItem Current { get; private set; }
}
