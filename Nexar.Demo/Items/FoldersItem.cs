using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FoldersItem(WorkspaceLibraryItem parent) : NodeTreeItem(parent)
{
    public new WorkspaceLibraryItem Parent { get; } = parent;
    public override string Text => "Folders";
    public override string Icon => Icons.Material.Outlined.SnippetFolder;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.Folders.ExecuteAsync(Parent.Tag.Url);
        var data = res.AssertNoErrors();

        var roots = FolderTreeNode.GetRootNodes(data.DesLibrary.Folders);

        return roots
            .Select(x => (TreeItem)new FolderItem(this, x))
            .ToList();
    }
}
