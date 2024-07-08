using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class ReleaseVariantItem(IMyReleaseVariant tag, ReleaseItem parent) : LeafTreeItem(parent)
{
    public IMyReleaseVariant Tag { get; } = tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Launch;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "ReleaseVariant";
    }

    public static event Action? OnChange;
    public static ReleaseVariantItem? Current { get; private set; }
}
