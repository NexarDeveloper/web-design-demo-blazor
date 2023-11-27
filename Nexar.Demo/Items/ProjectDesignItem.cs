using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectDesignItem(ProjectItem parent) : NodeTreeItem(parent)
{
    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Design";
    public override string Icon => Icons.Material.Filled.Memory;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.ProjectVariants.ExecuteAsync(Parent.Tag.Id);
        res.AssertNoErrors();

        return res.Data.DesProjectById.Design.Variants
            .Select(x => (TreeItem)new VariantItem(x, this))
            .ToHashSet();
    }
}
