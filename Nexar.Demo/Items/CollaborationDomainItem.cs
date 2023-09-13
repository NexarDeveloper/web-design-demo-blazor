﻿using MudBlazor;
using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class CollaborationDomainItem : LeafTreeItem
{
    public DesCollaborationDomain Domain { get; private set; }
    public IMyCollaborationRevision LatestRevision { get; private set; }
    public IReadOnlyList<IMyCollaborationRevision> Revisions { get; private set; }

    public CollaborationDomainItem(ProjectCollaborationItem parent, DesCollaborationDomain domain) : base(parent)
    {
        Domain = domain;
        Parent = parent;
    }

    public new ProjectCollaborationItem Parent { get; }
    public override string Text => Domain.ToString().ToUpper();
    public override string Icon => Icons.Material.Filled.CompareArrows;

    public override string SetCurrent()
    {
        if (Current != this)
        {
            Current = this;
            Update(() => OnChange?.Invoke());
        }
        return "CollaborationDomain";
    }

    public static event Action OnChange;
    public static CollaborationDomainItem Current { get; private set; }

    protected override async Task UpdateAsync()
    {
        var res = await Client.ProjectCollaboration.ExecuteAsync(Parent.Parent.Tag.Id, Domain);
        res.AssertNoErrors();

        LatestRevision = res.Data.LatestRevision;
        Revisions = res.Data.Revisions.Nodes.Reverse().ToList();
    }
}
