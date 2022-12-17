using Microsoft.AspNetCore.Components;
using Nexar.Client;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design.Shared;

public partial class NavMenu : IDisposable
{
    [Inject] NavigationManager NavManager { get; init; }

    protected override void OnInitialized()
    {
        AppData.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AppData.OnChange -= StateHasChanged;
        GC.SuppressFinalize(this);
    }

    static async Task<HashSet<TreeItem>> ServerData(TreeItem node)
    {
        try
        {
            if (node is WorkspaceItem workspaceItem)
            {
                var client = NexarClientFactory.GetClient(workspaceItem.Tag.Location.ApiServiceUrl);
                var res = await client.Projects.ExecuteAsync(workspaceItem.Tag.Url);
                res.EnsureNoErrors();

                workspaceItem.Items = res.Data.DesProjects.Nodes
                    .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                    .Select(x => (TreeItem)new ProjectItem(x, workspaceItem))
                    .ToHashSet();

                return workspaceItem.Items;
            }

            if (node is ProjectItem projectItem)
            {
                var client = NexarClientFactory.GetClient(projectItem.WorkspaceItem.Tag.Location.ApiServiceUrl);
                var res = await client.CommentThreads.ExecuteAsync(projectItem.Tag.Id);
                res.EnsureNoErrors();

                projectItem.Items = res.Data.DesCommentThreads
                    .OrderByDescending(x => x.ThreadNumber)
                    .Select(x => (TreeItem)new ThreadItem(x, projectItem))
                    .ToHashSet();

                return projectItem.Items;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return null;
    }

    void ActivatedValueChanged(TreeItem node)
    {
        if (node is WorkspaceItem workspace)
        {
            AppData.CurrentWorkspace = workspace;
            NavManager.NavigateTo("workspace");
            return;
        }

        if (node is ProjectItem project)
        {
            AppData.CurrentProject = project;
            NavManager.NavigateTo("project");
            return;
        }

        if (node is ThreadItem thread)
        {
            AppData.CurrentThread = thread;
            NavManager.NavigateTo("thread");
            return;
        }
    }
}
