using MudBlazor;
using Nexar.Client;
using System;
using System.Linq;

namespace Nexar.Demo;

public sealed class UserItem(IMyUser tag, WorkspaceUsersItem parent) : LeafTreeItem(parent)
{
    public IMyUser Tag { get; } = tag;
    public override string Text => Tag.UserName ?? string.Empty;
    public override string Icon => Icons.Material.Filled.Person;

    public string Groups => string.Join(", ", Tag.Groups!.Select(x => x.Name));

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "User";
    }

    public static event Action? OnChange;
    public static UserItem? Current { get; private set; }
}
