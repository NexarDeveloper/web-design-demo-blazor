using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantBomItem : LeafTreeItem
{
    public VariantBomItem(VariantItem parent) : base(parent)
    {
    }

    public IMyBom Tag { get; private set; }
    public override string Text => "BOM";
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
        return "VariantBom";
    }

    public static event Action OnChange;
    public static VariantBomItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.VariantBom.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name);
            res.AssertNoErrors();

            Tag = res.Data.DesProjectById.Design.Variants[0].Bom;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        OnChange?.Invoke();
    }
}
