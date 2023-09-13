using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ComponentTemplatesItem : LeafTreeItem
{
    public ComponentTemplatesItem(WorkspaceLibraryItem parent) : base(parent)
    {
    }

    public IReadOnlyList<IMyComponentTemplate> Templates { get; private set; }
    public override string Text => "Component Templates";
    public override string Icon => Icons.Material.Filled.CopyAll;
    public new WorkspaceLibraryItem Parent => (WorkspaceLibraryItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "ComponentTemplates";
    }

    public static event Action OnChange;
    public static ComponentTemplatesItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ComponentTemplates.ExecuteAsync(Parent.Parent.Tag.Url);
        res.AssertNoErrors();

        Templates = res.Data.DesLibrary.ComponentTemplates.Nodes;
    }
}
