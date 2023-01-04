using MudBlazor;
using StrawberryShake;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design;

public sealed class ProjectCommentsItem : TreeItem2
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
        res.EnsureNoErrors();

        return res.Data.DesCommentThreads
            .OrderByDescending(x => x.ThreadNumber)
            .Select(x => (TreeItem)new ThreadItem(x, this))
            .ToHashSet();
    }
}
