using System;
using System.Collections.Generic;

namespace Nexar.Design
{
    public class TreeNode
    {
        /// <summary>
        /// The item name shown in the tree.
        /// Null name is used for "Loading...".
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Node icon.
        /// </summary>
        public virtual object Icon { get; } = Blazorise.Icons.FontAwesome.FontAwesomeIcons.Folder;

        /// <summary>
        /// Child nodes. One node with null name means not fetched.
        /// </summary>
        public IReadOnlyList<TreeNode> Nodes { get; set; } = Array.Empty<TreeNode>();
    }

    public class WorkspaceNode : TreeNode
    {
        public IMyWorkspace Tag { get; }
        public WorkspaceNode(IMyWorkspace tag)
        {
            Tag = tag;
            Nodes = new List<TreeNode>() { new TreeNode() };
        }
        public override string Name =>
            Tag.Name;
    }

    public class ProjectNode : TreeNode
    {
        public IMyProject Tag { get; }
        public WorkspaceNode Workspace { get; }
        public ProjectNode(IMyProject tag, WorkspaceNode workspace)
        {
            Tag = tag;
            Workspace = workspace;
            Nodes = new List<TreeNode>() { new TreeNode() };
        }
        public override string Name =>
            Tag.Name;
        public override object Icon =>
            Blazorise.Icons.FontAwesome.FontAwesomeIcons.FileAlt;
    }

    public class ThreadNode : TreeNode
    {
        readonly string _name;
        public IMyThread Tag { get; }
        public ProjectNode Project { get; }
        public ThreadNode(IMyThread tag, ProjectNode project)
        {
            Tag = tag;
            Project = project;
            Nodes = Array.Empty<TreeNode>();

            if (Tag.Comments.Count == 0)
                _name = "<empty>";
            else
                _name = Tag.Comments[0].Text.Trim();
        }
        public override string Name =>
            _name;
        public override object Icon =>
            Blazorise.Icons.FontAwesome.FontAwesomeIcons.Comment;
    }
}
