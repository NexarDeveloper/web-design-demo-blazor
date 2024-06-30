using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

/// <summary>
/// Base tree item.
/// </summary>
public abstract class TreeItem
{
    private Exception _error;

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
    public virtual NexarClient Client => NexarClientFactory.GetClient(AppData.ApiEndpoint);

    /// <summary>
    /// Gets child items on the first expanding.
    /// </summary>
    public abstract Task<List<TreeItem>> ServerData();

    /// <summary>
    /// Sets this item current and tells to navigate to the returned page.
    /// </summary>
    public virtual string SetCurrent() => null;

    /// <summary>
    /// An error during processing.
    /// </summary>
    public Exception Error
    {
        get => _error;
        set
        {
            _error = value;
            if (value is not null)
                Console.WriteLine(value);
        }
    }

    public bool IsUpdating { get; private set; }

    protected virtual Task UpdateAsync()
    {
        throw new NotImplementedException();
    }

    protected void Update(Action onChange)
    {
        Error = null;
        IsUpdating = true;
        onChange?.Invoke();

        Task.Run(async () =>
        {
            try
            {
                await UpdateAsync();
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                IsUpdating = false;
                onChange?.Invoke();
            }
        });
    }
}

/// <summary>
/// Tree item with children.
/// </summary>
public abstract class NodeTreeItem(TreeItem parent) : TreeItem
{
    public TreeItem Parent { get; } = parent;
    public sealed override NexarClient Client => Parent.Client;
    public sealed override string Path => Parent.Path + " / " + Text;
}

/// <summary>
/// Tree item with no children.
/// </summary>
public abstract class LeafTreeItem(TreeItem parent) : NodeTreeItem(parent)
{
    public sealed override bool CanExpand => false;
    public sealed override Task<List<TreeItem>> ServerData() => throw new NotImplementedException();
}
