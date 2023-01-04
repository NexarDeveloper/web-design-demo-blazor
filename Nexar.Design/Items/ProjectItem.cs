using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class ProjectItem : TreeItem2
{
    public ProjectItem(IMyProject tag, WorkspaceProjectsItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyProject Tag { get; }
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Memory;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new ProjectDesignItem(this),
            new ProjectReleasesItem(this),
            new ProjectCommentsItem(this),
            new ProjectTasksItem(this),
        });
    }

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "project";
    }

    public static event Action OnChange;
    public static ProjectItem Current { get; private set; }
}
