using System.Collections.Generic;
using System.Linq;

namespace Nexar.Client
{
    public class FolderTreeNode
    {
        public IMyFolder Folder { get; }
        public List<FolderTreeNode> Nodes { get; }

        public FolderTreeNode(IMyFolder folder, List<FolderTreeNode> nodes)
        {
            Folder = folder;
            Nodes = nodes;
        }

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
                .Where(x => x.Parent?.Id == folder.Id)
                .OrderBy(x => x.Name)
                .Select(x => new FolderTreeNode(x, GetFolderNodes(x, folders)))
                .ToList();
        }
    }
}
