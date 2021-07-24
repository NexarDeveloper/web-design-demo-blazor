using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;

namespace Nexar.Design
{
    public abstract class TreeNode
    {
        public virtual string Name { get; }
        public abstract string Icon { get; }
        public HashSet<TreeNode> Nodes { get; set; }
    }

    public sealed class WorkspaceNode : TreeNode
    {
        public IMyWorkspace Tag { get; }
        public WorkspaceNode(IMyWorkspace tag)
        {
            Tag = tag;
        }
        public override string Name =>
            Tag.Name;
        public override string Icon =>
            Icons.Filled.FolderOpen;
    }

    public sealed class ProjectNode : TreeNode
    {
        public IMyProject Tag { get; }
        public WorkspaceNode Workspace { get; }
        public ProjectNode(IMyProject tag, WorkspaceNode workspace)
        {
            Tag = tag;
            Workspace = workspace;
        }
        public override string Name =>
            Tag.Name;
        public override string Icon =>
            Icons.Filled.Memory;
    }

    public sealed class ThreadNode : TreeNode
    {
        readonly string _name;
        public IMyThread Tag { get; }
        public ProjectNode Project { get; }
        public ThreadNode(IMyThread tag, ProjectNode project)
        {
            Tag = tag;
            Project = project;

            if (Tag.Comments.Count == 0)
                _name = "<empty>";
            else
                _name = Tag.Comments[0].Text.Trim();
        }
        public override string Name =>
            _name;
        public override string Icon =>
            Icons.Filled.ChatBubbleOutline;
    }
}
