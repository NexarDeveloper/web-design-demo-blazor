using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantSchematicsItem(VariantItem parent) : NodeTreeItem(parent)
{
    readonly VariantItem _parent = parent;
    public override string Text => "Schematics";
    public override string Icon => Icons.Material.Filled.Transform;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.VariantSchematics.ExecuteAsync(_parent.Parent.Parent.Tag.Id, _parent.Tag.Name, SchematicItem.ItemsLimit);
        res.AssertNoErrors();

        return res.Data.DesProjectById.Design.Variants[0].Schematics
            .Select(x => (TreeItem)new SchematicItem(x, this))
            .ToList();
    }
}
