using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectItem(IMyProject tag, WorkspaceProjectsItem parent) : NodeTreeItem(parent)
{
    public IMyProjectRevision? Revision { get; private set; }
    public IReadOnlyList<IMyProjectParameter>? Parameters { get; private set; }
    public IReadOnlyList<IMyProjectPermission>? Permissions { get; private set; }

    public IMyProject Tag { get; } = tag;
    public override string Text => Tag.Name ?? string.Empty;
    public override string Icon => Icons.Material.Filled.Memory;

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new ProjectDesignItem(this),
            new ProjectReleasesItem(this),
            new ProjectCollaborationItem(this),
            new ProjectSimulationItem(this),
            new ProjectTasksItem(this),
            new ProjectCommentsItem(this),
            new ProjectRevisionsItem(this),
        });
    }

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "Project";
    }

    public static event Action? OnChange;
    public static ProjectItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectExtras.ExecuteAsync(Tag.Id);
        var data = res.AssertNoErrors();

        Revision = data.DesProjectById?.LatestRevision;
        Parameters = data.DesProjectById?.Parameters;
        Permissions = data.DesProjectById?.ProjectPermissions;
    }
}
