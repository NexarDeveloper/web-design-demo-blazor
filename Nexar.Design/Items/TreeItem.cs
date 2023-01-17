using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Design;

/// <summary>
/// Base tree item.
/// </summary>
public abstract class TreeItem
{
    /// <summary>
    /// The tree text.
    /// </summary>
    public abstract string Text { get; }

    /// <summary>
    /// The tree icon.
    /// </summary>
    public abstract string Icon { get; }

    /// <summary>
    /// Gets the item path.
    /// </summary>
    public abstract string Path { get; }

    /// <summary>
    /// Gets true if the item may have children.
    /// </summary>
    public virtual bool CanExpand { get; } = true;

    /// <summary>
    /// Gets the client for fetching data.
    /// </summary>
    public abstract NexarClient Client { get; }

    /// <summary>
    /// Gets child items on the first expanding.
    /// </summary>
    public abstract Task<HashSet<TreeItem>> ServerData();

    /// <summary>
    /// Sets this item current and tells to navigate to the returned page.
    /// </summary>
    public virtual string SetCurrent() => null;
}

/// <summary>
/// Tree item with ability to expand.
/// </summary>
public abstract class TreeItem2 : TreeItem
{
    public TreeItem2(TreeItem parent)
    {
        Parent = parent;
    }
    public TreeItem Parent { get; }
    public sealed override NexarClient Client => Parent.Client;
    public sealed override string Path => Parent.Path + " / " + Text;
}

/// <summary>
/// Terminal tree item with no children.
/// </summary>
public abstract class TreeItem3 : TreeItem2
{
    public TreeItem3(TreeItem parent) : base(parent)
    {
    }
    public sealed override bool CanExpand => false;
    public sealed override Task<HashSet<TreeItem>> ServerData() => throw new NotImplementedException();
}
