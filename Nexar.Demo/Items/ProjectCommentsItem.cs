using MudBlazor;
using Nexar.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Demo;

public sealed class ProjectCommentsItem(ProjectItem parent) : NodeTreeItem(parent)
{
    readonly ProjectItem _parent = parent;
    public override string Text => "Comments";
    public override string Icon => Icons.Material.Filled.ChatBubbleOutline;

    public override async Task<List<TreeItem>> ServerData()
    {
        var res = await Client.CommentThreads.ExecuteAsync(_parent.Tag.Id);
        var data = res.AssertNoErrors();

        return data.DesCommentThreads
            .OrderByDescending(x => x.ThreadNumber)
            .Select(x => (TreeItem)new ThreadItem(x, this))
            .ToList();
    }
}
