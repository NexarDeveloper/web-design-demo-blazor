namespace Nexar.Client;

public class FolderTreeNode(IMyFolder folder, List<FolderTreeNode> nodes)
{
    public IMyFolder Folder { get; } = folder;
    public List<FolderTreeNode> Nodes { get; } = nodes;

    public static List<FolderTreeNode> GetRootNodes(IEnumerable<IMyFolder> folders)
    {
        return folders
            .Where(x => x.Parent is null)
            .OrderBy(x => x.Name)
            .Select(x => new FolderTreeNode(x, GetFolderNodes(x, folders)))
            .ToList();
    }

    public static List<FolderTreeNode> GetFolderNodes(IMyFolder folder, IEnumerable<IMyFolder> folders)
    {
        return folders
            .Where(x => x.Parent?.FolderId == folder.FolderId)
            .OrderBy(x => x.Name)
            .Select(x => new FolderTreeNode(x, GetFolderNodes(x, folders)))
            .ToList();
    }
}
