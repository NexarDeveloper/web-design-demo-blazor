using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Design;

public sealed class WipVariantItem : TreeItem3
{
    public WipVariantItem(IMyWipVariant tag)
    {
        Tag = tag;
    }

    public IMyWipVariant Tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Filled.Memory;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "wipVariant";
    }

    public static event Action OnChange;
    public static WipVariantItem Current { get; private set; }
}
