using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeProjectsItem : LeafTreeItem
{
    public IReadOnlyList<IMySharedWithMeProject> Projects { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; }

    public SharedWithMeProjectsItem(SharedWithMeItem parent) : base(parent)
    {
    }

    public override string Text => "Projects";
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "SharedWithMeProjects";
    }

    public static event Action OnChange;
    public static SharedWithMeProjectsItem Current { get; private set; }

    async Task Fetch()
    {
        var res = await Client.SharedWithMeProjects.ExecuteAsync();

        Projects = res.Data?.DesSharedWithMe?.Projects?
            .OrderBy(x => x.Name)
            .ToList();

        if (res.Errors.Count > 0)
            Errors = res.Errors.Select(x => x.Message).ToList();

        OnChange?.Invoke();
    }
}
