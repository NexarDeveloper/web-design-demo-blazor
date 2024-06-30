using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class SharedWithMeItem : TreeItem
{
    const string MyName = "Shared with Me";

    public override string Text => MyName;
    public override string Path => MyName;
    public override string Icon => Icons.Material.Filled.FolderShared;

    public override Task<List<TreeItem>> ServerData()
    {
        return Task.FromResult(new List<TreeItem>
        {
            new SharedWithMeProjectsItem(this),
        });
    }
}
