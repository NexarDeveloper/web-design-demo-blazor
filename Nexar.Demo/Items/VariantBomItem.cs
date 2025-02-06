using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantBomItem(NodeTreeItem parent) : LeafTreeItem(parent)
{
    public const int ItemsLimit = 100;

    public IMyBom? Tag { get; private set; }
    public List<MyData>? Items { get; private set; }
    public override string Text => "BOM";
    public override string Icon => Icons.Material.Filled.List;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "VariantBom";
    }

    public static event Action? OnChange;
    public static VariantBomItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        if (Parent is VariantItem variantItem)
        {
            var res = await Client.DesignVariantBom.ExecuteAsync(variantItem.Tag.Id, ItemsLimit);
            var data = res.AssertNoErrors();

            Tag = data.DesWipVariantById?.Bom;
        }
        else if (Parent is ReleaseVariantItem releaseVariantItem)
        {
            var res = await Client.ReleaseVariantBom.ExecuteAsync(releaseVariantItem.Tag.Id, ItemsLimit);
            var data = res.AssertNoErrors();

            Tag = data.DesReleaseVariantById?.Bom;
        }
        else
        {
            throw new NotImplementedException();
        }

        Items = Tag?.Items?.Nodes!
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
        public string? Name { get; set; }
        public bool IsManaged { get; set; }
        public int Quantity { get; set; }
        public IReadOnlyList<IMyBomItemInstance>? Instances { get; set; }
    }
}
