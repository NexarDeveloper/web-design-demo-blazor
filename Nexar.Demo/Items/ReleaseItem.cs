﻿using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ReleaseItem(IMyRelease tag, ProjectReleasesItem parent) : NodeTreeItem(parent)
{
    public IMyRelease Tag { get; } = tag;
    public override string Text => Tag.CreatedAt.ToString();
    public override string Icon => Icons.Material.Filled.Launch;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Release";
    }

    public static event Action OnChange;
    public static ReleaseItem Current { get; private set; }

    public override async Task<List<TreeItem>> ServerData()
    {
        try
        {
            var res = await Client.ProjectReleaseById.ExecuteAsync(Tag.Id);
            res.AssertNoErrors();

            return res.Data.DesReleaseById.Variants
                .Select(x => (TreeItem)new ReleaseVariantItem(x, this))
                .ToList();
        }
        catch
        {
            return [];
        }
    }
}
