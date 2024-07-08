using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FolderItem(NodeTreeItem parent, FolderTreeNode node) : NodeTreeItem(parent)
{
    readonly FolderTreeNode _node = node;

    public IMyFolderExtras? Extras { get; private set; }

    public IMyFolder Tag => _node.Folder;
    public override string Text => _node.Folder.Name;
    public override string Icon => Icons.Material.Outlined.SnippetFolder;

    public override bool CanExpand => _node.Nodes.Count > 0;

    public override Task<List<TreeItem>> ServerData()
    {
        var items = _node.Nodes
            .Select(x => (TreeItem)new FolderItem(this, x))
            .ToList();

        return Task.FromResult(items);
    }

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "Folder";
    }

    public static event Action? OnChange;
    public static FolderItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.FolderExtras.ExecuteAsync(Tag.Id);
        var data = res.AssertNoErrors();

        Extras = data.DesFolderById
            ?? throw new Exception("Cannot get folder extras.");
    }
}
