using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectCommentsItem : NodeTreeItem
{
    public ProjectCommentsItem(ProjectItem parent) : base(parent)
    {
        _parent = parent;
    }

    readonly ProjectItem _parent;
    public override string Text => "Comments";
    public override string Icon => Icons.Material.Filled.ChatBubbleOutline;

    public override async Task<HashSet<TreeItem>> ServerData()
    {
        var res = await Client.CommentThreads.ExecuteAsync(_parent.Tag.Id);
        res.AssertNoErrors();

        return res.Data.DesCommentThreads
            .OrderByDescending(x => x.ThreadNumber)
            .Select(x => (TreeItem)new ThreadItem(x, this))
            .ToHashSet();
    }
}
