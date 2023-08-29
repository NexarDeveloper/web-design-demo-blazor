﻿using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class FolderItem : NodeTreeItem
{
    readonly FolderTreeNode _node;

    public IMyFolderExtras Extras { get; private set; }

    public FolderItem(NodeTreeItem parent, FolderTreeNode node) : base(parent)
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
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "Folder";
    }

    public static event Action OnChange;
    public static FolderItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.FolderExtras.ExecuteAsync(Tag.Id);
            res.AssertNoErrors();

            Extras = res.Data.Node as IMyFolderExtras
                ?? throw new Exception("Cannot get folder extras.");
        }
        catch (Exception ex)
        {
            Error = ex;
            Extras = null;
        }

        OnChange?.Invoke();
    }
}
