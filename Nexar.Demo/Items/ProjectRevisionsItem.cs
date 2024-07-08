using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectRevisionsItem(ProjectItem parent) : LeafTreeItem(parent)
{
    public IReadOnlyList<IMyRevision>? Revisions { get; private set; }

    public new ProjectItem Parent { get; } = parent;
    public override string Text => "Revisions";
    public override string Icon => Icons.Material.Filled.Checklist;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "Revisions";
    }

    public static event Action? OnChange;
    public static ProjectRevisionsItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectRevisions.ExecuteAsync(Parent.Tag.Id);
        var data = res.AssertNoErrors();

        Revisions = data.DesProjectById!.Revisions?.Nodes;
    }
}
