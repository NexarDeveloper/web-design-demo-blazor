﻿using MudBlazor;
using Nexar.Client;
using System;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class VariantLayersItem(VariantItem parent) : LeafTreeItem(parent)
{
    public IMyStackup? Tag { get; private set; }
    public override string Text => "Layers";
    public override string Icon => Icons.Material.Outlined.Layers;
    public new VariantItem Parent => (VariantItem)base.Parent;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "VariantLayers";
    }

    public static event Action? OnChange;
    public static VariantLayersItem? Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.DesignVariantLayers.ExecuteAsync(Parent.Tag.Id);
        var data = res.AssertNoErrors();

        var pcb = data.DesWipVariantById?.Pcb;
        if (pcb is null)
            return;

        Tag = pcb.LayerStack
            ?? throw new Exception("Cannot get layers.");
    }
}
