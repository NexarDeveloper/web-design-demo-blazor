using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectTemplatesItem(WorkspaceLibraryItem parent) : LeafTreeItem(parent)
{
    public IReadOnlyList<IMyProjectTemplate>? Templates { get; private set; }
    public override string Text => "Project Templates";
    public override string Icon => Icons.Material.Filled.CopyAll;
    public new WorkspaceLibraryItem Parent => (WorkspaceLibraryItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "ProjectTemplates";
    }

    public static event Action? OnChange;
    public static ProjectTemplatesItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectTemplates.ExecuteAsync(Parent.Parent.Tag.Url);
        var data = res.AssertNoErrors();

        Templates = data.DesLibrary.ProjectTemplates?.Nodes;
    }
}
