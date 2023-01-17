﻿using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Design;

public sealed class ReleaseItem : TreeItem3
{
    public ReleaseItem(IMyRelease tag, ProjectReleasesItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMyRelease Tag;
    public override string Text => Tag.CreatedAt.ToString();
    public override string Icon => Icons.Material.Filled.ContentPasteGo;
    public override bool CanExpand => false;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "release";
    }

    public static event Action OnChange;
    public static ReleaseItem Current { get; private set; }
}
