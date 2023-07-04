using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeProjectsItem : NodeTreeItem
{
    public HashSet<TreeItem> Items { get; private set; }

    public SharedWithMeProjectsItem(SharedWithMeItem parent) : base(parent)
    {
    }

    public override string Text => "Projects";
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "SharedWithMeProjects";
    }

    public static event Action OnChange;
    public static SharedWithMeProjectsItem Current { get; private set; }

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.SharedWithMeProjects.ExecuteAsync();
        res.AssertNoErrors();

        Items = res.Data.DesSharedWithMe.Projects.Nodes
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new SharedWithMeProjectItem(x.ProjectId, x.Name, this))
            .ToHashSet();

        return Items;
    }
}
