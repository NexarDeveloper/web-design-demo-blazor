using MudBlazor;
using Nexar.Client;
using System;
using System.Linq;

namespace Nexar.Design;

public sealed class UserItem : TreeItem3
{
    public UserItem(IMyUser tag, WorkspaceUsersItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyUser Tag { get; }
    public override string Text => Tag.UserName;
    public override string Icon => Icons.Filled.Person;

    public string Groups => string.Join(", ", Tag.Groups.Select(x => x.Name));

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "user";
    }

    public static event Action OnChange;
    public static UserItem Current { get; private set; }
}
