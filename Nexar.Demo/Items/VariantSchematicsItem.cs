using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantSchematicsItem : NodeTreeItem
{
    public VariantSchematicsItem(VariantItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly VariantItem _parent;
    public override string Text => "Schematics";
    public override string Icon => Icons.Material.Filled.Transform;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.VariantSchematics.ExecuteAsync(_parent.Parent.Parent.Tag.Id, _parent.Tag.Name);
        res.AssertNoErrors();

        return res.Data.DesProjectById.Design.Variants[0].Schematics
            .Select(x => (TreeItem)new SchematicItem(x, this))
            .ToHashSet();
    }
}
