﻿using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class WipVariantItem : TreeItem3
{
    public WipVariantItem(IMyWipVariant tag, ProjectDesignItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyWipVariant Tag;
    public override string Text => Tag.Name;
    public override string Icon => Icons.Material.Filled.Memory;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "wipVariant";
    }

    public static event Action OnChange;
    public static WipVariantItem Current { get; private set; }
}