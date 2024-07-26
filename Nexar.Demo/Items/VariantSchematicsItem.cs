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
            var res = await Client.VariantSchematics.ExecuteAsync(variantItem.Parent.Parent.Tag.Id, variantItem.Tag.Name, SchematicItem.ItemsLimit);
            var data = res.AssertNoErrors();

            return data.DesProjectById!.Design.Variants[0].Schematics
                .Select(x => (TreeItem)new SchematicItem(x, this))
                .ToList();
        }
        else if (Parent is ReleaseVariantItem releaseVariantItem)
        {
            var res = await Client.ReleaseVariantSchematics.ExecuteAsync(releaseVariantItem.Parent.Tag.Id, releaseVariantItem.Tag.Name, SchematicItem.ItemsLimit);
            var data = res.AssertNoErrors();

            return data.DesReleaseById!.Variants[0].Schematics
                .Select(x => (TreeItem)new SchematicItem(x, this))
                .ToList();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
