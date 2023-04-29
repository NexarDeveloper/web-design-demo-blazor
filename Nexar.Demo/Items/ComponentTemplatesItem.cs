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
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "ComponentTemplates";
    }

    public static event Action OnChange;
    public static ComponentTemplatesItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.ComponentTemplates.ExecuteAsync(Parent.Parent.Tag.Url);
            res.AssertNoErrors();

            Templates = res.Data.DesLibrary.ComponentTemplates.Nodes;
        }
        catch
        {
            Templates = Array.Empty<IMyComponentTemplate>();
        }

        OnChange?.Invoke();
    }
}
