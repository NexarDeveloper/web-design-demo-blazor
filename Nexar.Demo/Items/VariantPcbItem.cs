using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantPcbItem(NodeTreeItem parent) : LeafTreeItem(parent)
{
    public const int ItemsLimit = 100;

    public IMyPcb? Tag { get; private set; }
    public override string Text => "PCB";
    public override string Icon => Icons.Material.Filled.List;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "VariantPcb";
    }

    public static event Action? OnChange;
    public static VariantPcbItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        if (Parent is VariantItem variantItem)
        {
            var res = await Client.VariantPcb.ExecuteAsync(variantItem.Parent.Parent.Tag.Id, variantItem.Tag.Name, ItemsLimit);
            var data = res.AssertNoErrors();

            Tag = data.DesProjectById!.Design.Variants[0].Pcb;
        }
        else if (Parent is ReleaseVariantItem releaseVariantItem)
        {
            var res = await Client.ReleaseVariantPcb.ExecuteAsync(releaseVariantItem.Parent.Tag.Id, releaseVariantItem.Tag.Name, ItemsLimit);
            var data = res.AssertNoErrors();

            Tag = data.DesReleaseById?.Variants[0].Pcb;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
