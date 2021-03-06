using Microsoft.AspNetCore.Components;
using Nexar.Client;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design.Shared
{
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

        async Task<HashSet<TreeItem>> ServerData(TreeItem node)
        {
            try
            {
                if (node is WorkspaceItem workspace)
                {
                    var client = NexarClientFactory.GetClient(workspace.Tag.Location.ApiServiceUrl);
                    var res = await client.Projects.ExecuteAsync(workspace.Tag.Url);
                    res.EnsureNoErrors();

                    workspace.Items = res.Data.DesProjects.Nodes
                        .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                        .Select(x => (TreeItem)new ProjectItem(x, workspace))
                        .ToHashSet();

                    return workspace.Items;
                }

                if (node is ProjectItem project)
                {
                    var client = NexarClientFactory.GetClient(project.Workspace.Tag.Location.ApiServiceUrl);
                    var res = await client.CommentThreads.ExecuteAsync(project.Tag.Id);
                    res.EnsureNoErrors();

                    project.Items = res.Data.DesCommentThreads
                        .OrderByDescending(x => x.ThreadNumber)
                        .Select(x => (TreeItem)new ThreadItem(x, project))
                        .ToHashSet();

                    return project.Items;
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
}
