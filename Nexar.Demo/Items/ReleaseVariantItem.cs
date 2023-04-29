using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class ReleaseVariantItem : LeafTreeItem
{
    public ReleaseVariantItem(IMyReleaseVariant tag, ReleaseItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyReleaseVariant Tag { get; }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Launch;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "ReleaseVariant";
    }

    public static event Action OnChange;
    public static ReleaseVariantItem Current { get; private set; }
}
