using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeProjectItem : LeafTreeItem
{
    public string ProjectId { get; }
    readonly string _projectName;

    public SharedWithMeProjectItem(string projectId, string projectName, SharedWithMeProjectsItem parent) : base(parent)
    {
        ProjectId = projectId;
        _projectName = projectName;
    }

    public IMySharedWithMeProject Tag { get; private set; }
    public override string Text => _projectName;
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "SharedWithMeProject";
    }

    public static event Action OnChange;
    public static SharedWithMeProjectItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.SharedWithMeProject.ExecuteAsync(ProjectId);
            res.AssertNoErrors();

            Tag = res.Data.DesProjectById;
            if (Tag is null)
                throw new Exception("Cannot get project data.");
        }
        catch (Exception ex)
        {
            Error = ex;
        }

        OnChange?.Invoke();
    }
}
