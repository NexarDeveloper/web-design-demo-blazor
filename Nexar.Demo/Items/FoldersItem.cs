﻿using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FoldersItem : NodeTreeItem
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

        var roots = FolderTreeNode.GetRootNodes(res.Data.DesLibrary.Folders);

        return roots
            .Select(x => (TreeItem)new FolderItem(this, x))
            .ToHashSet();
    }
}
