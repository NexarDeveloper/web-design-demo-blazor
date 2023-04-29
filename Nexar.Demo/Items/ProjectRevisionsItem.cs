using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectRevisionsItem : TreeItem3
{
    public IReadOnlyList<IMyRevision> Revisions { get; private set; }

    public ProjectRevisionsItem(ProjectItem parent) : base(parent)
    {
        Parent = parent;
    }

    public new ProjectItem Parent { get; }
    public override string Text => "Revisions";
    public override string Icon => Icons.Material.Filled.Checklist;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "Revisions";
    }

    public static event Action OnChange;
    public static ProjectRevisionsItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.ProjectRevisions.ExecuteAsync(Parent.Tag.Id);
            res.AssertNoErrors();

            Revisions = res.Data.DesProjectById.Revisions.Nodes;
        }
        catch
        {
            Revisions = Array.Empty<IMyRevision>();
        }

        OnChange?.Invoke();
    }
}
