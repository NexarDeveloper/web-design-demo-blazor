using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class WorkflowItem(IMyWorkflow tag, WorkspaceWorkflowsItem parent) : LeafTreeItem(parent)
{
    public IMyWorkflow Tag { get; } = tag;
    public override string Text => Tag.Name ?? string.Empty;
    public override string Icon => Icons.Material.Outlined.AccountTree;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Workflow";
    }

    public static event Action? OnChange;
    public static WorkflowItem? Current { get; private set; }
}
