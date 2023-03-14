using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceUsersItem : TreeItem2
{
    public WorkspaceUsersItem(WorkspaceItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly WorkspaceItem _parent;
    public override string Text => "Users";
    public override string Icon => Icons.Material.Filled.Person;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.TeamUsers.ExecuteAsync(_parent.Tag.Url);
        res.AssertNoErrors();

        return res.Data.DesTeam.Users
            .OrderBy(x => x.UserName, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new UserItem(x, this))
            .ToHashSet();
    }
}
