using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class SchematicItem(IMySchematic tag, VariantSchematicsItem parent) : LeafTreeItem(parent)
{
    public const int ItemsLimit = 100;

    public IMySchematic Tag { get; } = tag;
    public override string Text => Tag.DocumentName;
    public override string Icon => Icons.Material.Filled.Transform;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Schematic";
    }

    public static event Action? OnChange;
    public static SchematicItem? Current { get; private set; }
}
