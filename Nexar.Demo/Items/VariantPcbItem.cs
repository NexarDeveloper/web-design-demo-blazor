using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantPcbItem(VariantItem parent) : LeafTreeItem(parent)
{
    public const int ItemsLimit = 100;

    public IMyPcb Tag { get; private set; }
    public override string Text => "PCB";
    public override string Icon => Icons.Material.Filled.List;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "VariantPcb";
    }

    public static event Action OnChange;
    public static VariantPcbItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.VariantPcb.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name, ItemsLimit);
        res.AssertNoErrors();

        Tag = res.Data.DesProjectById.Design.Variants[0].Pcb;
    }
}
