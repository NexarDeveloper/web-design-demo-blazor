using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class VariantItem(IMyWipVariant tag, ProjectDesignItem parent) : NodeTreeItem(parent)
{
    public IMyWipVariant Tag { get; } = tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Variant";
    }

    public static event Action? OnChange;
    public static VariantItem? Current { get; private set; }

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new VariantBomItem(this),
            new VariantPcbItem(this),
            new VariantLayersItem(this),
            new VariantSchematicsItem(this),
        });
    }
}
