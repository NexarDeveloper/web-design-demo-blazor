using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class ReleaseVariantItem(IMyReleaseVariant tag, ReleaseItem parent) : NodeTreeItem(parent)
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
