using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FolderItem : TreeItem2
{
    readonly FoldersItem.FolderTreeNode _node;

    public FolderItem(TreeItem2 parent, FoldersItem.FolderTreeNode node) : base(parent)
    {
        _node = node;
    }

    public IMyFolder Tag => _node.Folder;
    public override string Text => _node.Folder.Name;
    public override string Icon => Icons.Material.Outlined.SnippetFolder;

    public override bool CanExpand => _node.Nodes.Count > 0;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        var items = _node.Nodes
            .Select(x => (TreeItem)new FolderItem(this, x))
            .ToHashSet();

        return Task.FromResult(items);
    }

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "folder";
    }

    public static event Action OnChange;
    public static FolderItem Current { get; private set; }
}
