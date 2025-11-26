using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

public sealed class WorkspaceUsersItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    public IMyUser? Tag { get; private set; }
    readonly WorkspaceItem _parent = parent;
    public override string Text => "Users";
    public override string Icon => Icons.Material.Filled.Person;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.TeamUsers.ExecuteAsync(_parent.Tag.Url);
        var data = res.AssertNoErrors();

        return data.DesTeam.Users
            .OrderBy(x => x.UserName, StringComparer.OrdinalIgnoreCase)
            .Select(x => (TreeItem)new UserItem(x, this))
            .ToList();
    }

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "Users";
    }

    public static event Action? OnChange;
    public static WorkspaceUsersItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.UserByAuth.ExecuteAsync();
        var data = res.AssertNoErrors();
        Tag = data.DesUserByAuth;
    }
}
