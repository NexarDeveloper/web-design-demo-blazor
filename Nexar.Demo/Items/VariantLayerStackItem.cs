using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantLayerStackItem : LeafTreeItem
{
    public VariantLayerStackItem(VariantItem parent) : base(parent)
    {
    }

    public IMyStackup Tag { get; private set; }
    public override string Text => "Layer Stack";
    public override string Icon => Icons.Material.Outlined.Layers;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "VariantLayerStack";
    }

    public static event Action OnChange;
    public static VariantLayerStackItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.VariantLayerStack.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name);
            res.AssertNoErrors();

            Tag = res.Data.DesProjectById.Design.WorkInProgress.Variants[0].Pcb.LayerStack;
        }
        catch
        {
        }

        OnChange?.Invoke();
    }
}
