using MudBlazor;

namespace Nexar.Demo;

public class MyTreeItemData : TreeItemData<TreeItem>
{
    public MyTreeItemData(TreeItem value) : base(value)
    {
        Text = value.Text;
        Icon = value.Icon;
        Expandable = value.CanExpand;
    }
}
