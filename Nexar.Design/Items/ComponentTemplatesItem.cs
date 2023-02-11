using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nexar.Design;

public sealed class ComponentTemplatesItem : TreeItem3
{
    public IReadOnlyList<IMyComponentTemplate> Templates { get; private set; }

    public ComponentTemplatesItem(WorkspaceLibraryItem parent) : base(parent)
    {
    }

    public new WorkspaceLibraryItem Parent => (WorkspaceLibraryItem)base.Parent;
    public override string Text => "Component Templates";
    public override string Icon => Icons.Material.Filled.CopyAll;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "component-templates";
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

            OnChange?.Invoke();
        }
        catch
        {
        }

        await Task.CompletedTask;
    }
}
