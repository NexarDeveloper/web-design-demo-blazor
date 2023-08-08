using MudBlazor;
using Nexar.Client;
using System;

namespace Nexar.Demo;

public sealed class SchematicItem : LeafTreeItem
{
    public SchematicItem(IMySchematic tag, VariantSchematicsItem parent) : base(parent)
    {
        Tag = tag;
    }

    public IMySchematic Tag { get; }
    public override string Text => Tag.DocumentName;
    public override string Icon => Icons.Material.Filled.Transform;

    public override string SetCurrent()
    {
        Current = this;
        OnChange?.Invoke();
        return "Schematic";
    }

    public static event Action OnChange;
    public static SchematicItem Current { get; private set; }
}
