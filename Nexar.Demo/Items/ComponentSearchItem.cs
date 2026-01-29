using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ComponentSearchItem(WorkspaceLibraryItem parent) : LeafTreeItem(parent)
{
    public override string Text => "Component Search";
    public override string Icon => Icons.Material.Filled.Search;
    public new WorkspaceLibraryItem Parent => (WorkspaceLibraryItem)base.Parent;
    public IReadOnlyList<IMyComponent>? Components { get; set; }

    public override string SetCurrent()
    {
        Current = this;
        return "ComponentSearch";
    }

    public static ComponentSearchItem? Current { get; private set; }

    public async Task SearchById(string id)
    {
        var res = await Client.ComponentById.ExecuteAsync(id);
        var data = res.AssertNoErrors();
        Components = data.DesComponentById is { } x ? [x] : null;
    }

    public async Task SearchByName(string name)
    {
        var res = await Client.LibraryComponentsByName.ExecuteAsync(Parent.Tag.Url, name);
        var data = res.AssertNoErrors();
        Components = data.DesLibrary.Components?.Nodes;
    }
}
