using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ReleaseVariantItem(IMyReleaseVariant tag, ReleaseItem parent) : NodeTreeItem(parent)
{
    public IMyReleaseVariant Tag { get; } = tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Launch;
    public new ReleaseItem Parent => (ReleaseItem)base.Parent;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "ReleaseVariant";
    }

    public static event Action? OnChange;
    public static ReleaseVariantItem? Current { get; private set; }

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new VariantBomItem(this),
            new VariantPcbItem(this),
            new VariantSchematicsItem(this),
        });
    }
}
