using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;

namespace Nexar.Design;

public abstract class TreeItem
{
    public abstract string Text { get; }
    public abstract string Icon { get; }
    public HashSet<TreeItem> Items { get; set; }
}

public sealed class WorkspaceItem : TreeItem
{
    public IMyWorkspace Tag { get; }
    public WorkspaceItem(IMyWorkspace tag)
    {
        Tag = tag;
    }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Filled.FolderOpen;
}

public sealed class ProjectItem : TreeItem
{
    public IMyProject Tag { get; }
    public WorkspaceItem WorkspaceItem { get; }
    public ProjectItem(IMyProject tag, WorkspaceItem workspaceItem)
    {
        Tag = tag;
        WorkspaceItem = workspaceItem;
    }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Filled.Memory;
}

public sealed class ThreadItem : TreeItem
{
    readonly string _name;
    public IMyThread Tag { get; }
    public ProjectItem ProjectItem { get; }
    public ThreadItem(IMyThread tag, ProjectItem projectItem)
    {
        Tag = tag;
        ProjectItem = projectItem;
        _name = Tag.Comments.Count == 0 ? "<empty>" : Tag.Comments[0].Text.Trim();
    }
    public override string Text => _name;
    public override string Icon => Icons.Filled.ChatBubbleOutline;
}
