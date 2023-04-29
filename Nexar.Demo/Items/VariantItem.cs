﻿using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantItem : TreeItem2
{
    public VariantItem(IMyWipVariant tag, ProjectDesignItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyWipVariant Tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Memory;
    public new ProjectDesignItem Parent => (ProjectDesignItem)base.Parent;

    public override Task<HashSet<TreeItem>> ServerData()
    {
        return Task.FromResult(new HashSet<TreeItem>
        {
            new VariantBomItem(this),
            new VariantLayerStackItem(this),
        });
    }
}
