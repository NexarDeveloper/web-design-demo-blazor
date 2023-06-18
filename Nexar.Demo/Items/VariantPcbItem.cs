using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantPcbItem : LeafTreeItem
{
    public const int ItemsLimit = 100;

    public VariantPcbItem(VariantItem parent) : base(parent)
    {
    }

    public IMyPcb Tag { get; private set; }
    public override string Text => "PCB";
    public override string Icon => Icons.Material.Filled.List;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "VariantPcb";
    }

    public static event Action OnChange;
    public static VariantPcbItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.VariantPcb.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name, ItemsLimit);
            res.AssertNoErrors();

            Tag = res.Data.DesProjectById.Design.Variants[0].Pcb;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        OnChange?.Invoke();
    }
}
