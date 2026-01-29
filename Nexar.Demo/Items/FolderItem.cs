using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class FolderItem : NodeTreeItem
{
    private readonly FolderTreeNode _node;
    private readonly string _workspaceUrl;

    public IMyFolderExtras? Extras { get; private set; }

    public IMyFolder Tag => _node.Folder;
    public override string Text => _node.Folder.Name;
    public override string Icon => Icons.Material.Outlined.SnippetFolder;

    public override bool CanExpand => _node.Nodes.Count > 0;

    public FolderItem(NodeTreeItem parent, FolderTreeNode node) : base(parent)
    {
        _node = node;

        _workspaceUrl = parent switch
        {
            FoldersItem item1 => item1.Parent.Parent.Tag.Url,
            FolderItem item2 => item2._workspaceUrl,
            _ => null!
        };
    }

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
        var res = await Client.FolderExtras.ExecuteAsync(_workspaceUrl, Tag.FolderId);
        var data = res.AssertNoErrors();

        Extras = data.DesFolderByFolderId
            ?? throw new Exception("Cannot get folder extras.");
    }
}
