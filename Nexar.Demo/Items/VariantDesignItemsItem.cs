using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantDesignItemsItem : LeafTreeItem
{
    public const int ItemsLimit = 1000;

    public VariantDesignItemsItem(VariantItem parent) : base(parent)
    {
    }

    public IReadOnlyList<IMyDesignItem> Items { get; private set; }
    public int TotalCount { get; private set; }

    public override string Text => "Design Items";
    public override string Icon => Icons.Material.Filled.List;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            _ = Fetch();
            Current = this;
            OnChange?.Invoke();
        }
        return "VariantDesignItems";
    }

    public static event Action OnChange;
    public static VariantDesignItemsItem Current { get; private set; }

    async Task Fetch()
    {
        try
        {
            var res = await Client.VariantDesignItems.ExecuteAsync(Parent.Parent.Parent.Tag.Id, Parent.Tag.Name, ItemsLimit);
            res.AssertNoErrors();

            var items = res.Data.DesProjectById.Design.Variants[0].Pcb.DesignItems;
            TotalCount = items.TotalCount;
            Items = items.Nodes;
        }
        catch
        {
        }

        OnChange?.Invoke();
    }
}
