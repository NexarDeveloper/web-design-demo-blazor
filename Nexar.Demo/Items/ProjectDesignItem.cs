using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectDesignItem(ProjectItem parent) : NodeTreeItem(parent)
{
    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Design";
    public override string Icon => Icons.Material.Filled.Memory;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.ProjectVariants.ExecuteAsync(Parent.Tag.Id);
        var data = res.AssertNoErrors();

        foreach (var item in data.DesProjectById!.Design.Variants)
        {
            Console.WriteLine(item.SystemDiagram?.SystemDiagramFile?.DownloadUrl);
        }

        return data.DesProjectById!.Design.Variants
            .Select(x => (TreeItem)new VariantItem(x, this))
            .ToList();
    }
}
