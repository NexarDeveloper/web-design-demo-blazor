using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantSchematicsItem(NodeTreeItem parent) : NodeTreeItem(parent)
{
    public override string Text => "Schematics";
    public override string Icon => Icons.Material.Filled.Transform;

    public override async Task<List<TreeItem>> ServerData()
    {
        if (Parent is VariantItem variantItem)
        {
            var res = await Client.DesignVariantSchematics.ExecuteAsync(variantItem.Tag.Id, SchematicItem.ItemsLimit);
            var data = res.AssertNoErrors();

            return data.DesWipVariantById!.Schematics
                .Select(x => (TreeItem)new SchematicItem(x, this))
                .ToList();
        }
        else if (Parent is ReleaseVariantItem releaseVariantItem)
        {
            var res = await Client.ReleaseVariantSchematics.ExecuteAsync(releaseVariantItem.Tag.Id, SchematicItem.ItemsLimit);
            var data = res.AssertNoErrors();

            return data.DesReleaseVariantById!.Schematics
                .Select(x => (TreeItem)new SchematicItem(x, this))
                .ToList();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
