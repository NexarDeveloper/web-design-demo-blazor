using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeItem : TreeItem
{
    const string MyName = "Shared with Me";

    public SharedWithMeItem()
    {
    }

    public override string Text => MyName;
    public override string Path => MyName;
    public override string Icon => Icons.Material.Filled.FolderShared;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new SharedWithMeProjectsItem(this),
        });
    }
}
