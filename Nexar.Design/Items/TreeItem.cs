using Nexar.Client;
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
/// Tree item with the parent and ability to expand.
/// </summary>
public abstract class TreeItem2 : TreeItem
{
    public TreeItem2(TreeItem parent)
    {
        _parent = parent;
    }
    readonly TreeItem _parent;
    public sealed override NexarClient Client => _parent.Client;
}

/// <summary>
/// Terminal tree item with no children and fetching.
/// </summary>
public abstract class TreeItem3 : TreeItem
{
    public sealed override NexarClient Client => throw new System.NotImplementedException();
    public sealed override Task<HashSet<TreeItem>> ServerData() => Task.FromResult(new HashSet<TreeItem>());
}
