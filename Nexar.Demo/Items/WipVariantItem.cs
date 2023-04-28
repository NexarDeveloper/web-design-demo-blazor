using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WipVariantItem : TreeItem2
{
    public WipVariantItem(IMyWipVariant tag, ProjectDesignItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyWipVariant Tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Memory;
    public new ProjectDesignItem Parent => (ProjectDesignItem)base.Parent;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "wipVariant";
    }

    public static event Action OnChange;
    public static WipVariantItem Current { get; private set; }

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new LayerStackItem(this),
        });
    }
}
