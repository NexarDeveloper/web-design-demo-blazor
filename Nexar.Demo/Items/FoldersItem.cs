using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FoldersItem : TreeItem2
{
    public FoldersItem(WorkspaceLibraryItem parent) : base(parent)
    {
        Parent = parent;
    }

    public new WorkspaceLibraryItem Parent { get; }
    public override string Text => "Folders";
    public override string Icon => Icons.Material.Outlined.SnippetFolder;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.Folders.ExecuteAsync(Parent.Tag.Url);
        res.AssertNoErrors();

        var folders = res.Data.DesLibrary.Folders;
        var roots = folders
            .Where(x => x.Parent is null)
            .Select(x => new FolderTreeNode(x, GetFolderNodes(x, folders)))
            .ToList();

        return roots
            .Select(x => (TreeItem)new FolderItem(this, x))
            .ToHashSet();
    }

    public record FolderTreeNode(IMyFolder Folder, List<FolderTreeNode> Nodes);

    private static List<FolderTreeNode> GetFolderNodes(IMyFolder folder, IEnumerable<IMyFolder> folders)
    {
        return folders
            .Where(x => x.Parent?.Id == folder.Id)
            .Select(x => new FolderTreeNode(x, GetFolderNodes(x, folders)))
            .ToList();
    }
}
