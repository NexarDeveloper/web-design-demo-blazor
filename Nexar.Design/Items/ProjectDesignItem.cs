using MudBlazor;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class ProjectDesignItem : TreeItem2
{
    public ProjectDesignItem(ProjectItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly ProjectItem _parent;
    public override string Text => "Design";
    public override string Icon => Icons.Filled.Memory;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.ProjectWipVariants.ExecuteAsync(_parent.Tag.Id);
        res.EnsureNoErrors();

        return res.Data.DesProjectById.Design.WorkInProgress.Variants
            .OrderByDescending(x => x.Name)
            .Select(x => (TreeItem)new WipVariantItem(x))
            .ToHashSet();
    }
}
