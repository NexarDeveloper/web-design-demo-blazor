using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class WorkspaceLibraryItem(WorkspaceItem parent) : NodeTreeItem(parent)
{
    public IMyWorkspace Tag => Parent.Tag;
    public override string Text => "Library";
    public override string Icon => Icons.Material.Filled.FolderOpen;
    public new WorkspaceItem Parent => (WorkspaceItem)base.Parent;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new FoldersItem(this),
            new ComponentTemplatesItem(this),
        });
    }
}
