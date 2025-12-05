using MudBlazor;
using Nexar.Client;

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

            if (data.DesWipVariantById is { } variant)
            {
                return variant.Schematics
                    .Select(x => (TreeItem)new SchematicItem(x, this))
                    .ToList();
            }
            return [];
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
