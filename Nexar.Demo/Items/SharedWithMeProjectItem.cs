using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeProjectItem(string projectId, string projectName, SharedWithMeProjectsItem parent) : LeafTreeItem(parent)
{
    public string ProjectId { get; } = projectId;
    readonly string _projectName = projectName;

    public IMySharedWithMeProject Tag { get; private set; }
    public override string Text => _projectName;
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "SharedWithMeProject";
    }

    public static event Action OnChange;
    public static SharedWithMeProjectItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.SharedWithMeProject.ExecuteAsync(ProjectId);
        res.AssertNoErrors();

        Tag = res.Data.DesProjectById;
        if (Tag is null)
            throw new Exception("Cannot get project data.");
    }
}
