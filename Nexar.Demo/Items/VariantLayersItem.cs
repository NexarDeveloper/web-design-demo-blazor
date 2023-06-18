using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantLayersItem : LeafTreeItem
{
    public VariantLayersItem(VariantItem parent) : base(parent)
    {
    }

    public IMyStackup Tag { get; private set; }
    public override string Text => "Layers";
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
        return "VariantLayers";
    }

    public static event Action OnChange;
    public static VariantLayersItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.VariantLayers.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name);
            res.AssertNoErrors();

            Tag = res.Data.DesProjectById.Design.Variants[0].Pcb.LayerStack;
            if (Tag is null)
                throw new Exception("Cannot get layers.");
        }
        catch (Exception ex)
        {
            Error = ex;
        }

        OnChange?.Invoke();
    }
}
