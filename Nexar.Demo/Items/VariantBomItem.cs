using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantBomItem(VariantItem parent) : LeafTreeItem(parent)
{
    public const int ItemsLimit = 100;

    public IMyBom Tag { get; private set; }
    public List<MyData> Items { get; private set; }
    public override string Text => "BOM";
    public override string Icon => Icons.Material.Filled.List;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "VariantBom";
    }

    public static event Action OnChange;
    public static VariantBomItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.VariantBom.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name, ItemsLimit);
        res.AssertNoErrors();

        Tag = res.Data.DesProjectById.Design.Variants[0].Bom;

        Items = Tag?.Items.Nodes
            .Select(x => new MyData
            {
                Name = x.Component.Name,
                IsManaged = x.Component.IsManaged,
                Quantity = x.Quantity,
                Instances = x.BomItemInstances.OrderBy(x => x.Designator).ToList()
            })
            .ToList();
    }

    public class MyData
    {
        public bool ShowDetails { get; set; }
        public string Name { get; set; }
        public bool IsManaged { get; set; }
        public int Quantity { get; set; }
        public IReadOnlyList<IMyBomItemInstance> Instances { get; set; }
    }
}
