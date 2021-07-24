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
        [Inject]
        NavigationManager NavManager { get; init; }

        [Inject]
        AppData AppData { get; init; }

        [Inject]
        NexarClient Client { get; init; }

        protected override void OnInitialized()
        {
            AppData.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            AppData.OnChange -= StateHasChanged;
            GC.SuppressFinalize(this);
        }

        async Task<HashSet<TreeNode>> ServerDataAsync(TreeNode node)
        {
            try
            {
                if (node is WorkspaceNode workspace)
                {
                    var res = await Client.Projects.ExecuteAsync(workspace.Tag.Url);
                    res.EnsureNoErrors();

                    workspace.Nodes = res.Data.DesProjects.Nodes
                        .Select(x => (TreeNode)new ProjectNode(x, workspace))
                        .ToHashSet();

                    return workspace.Nodes;
                }

                if (node is ProjectNode project)
                {
                    var res = await Client.CommentThreads.ExecuteAsync(project.Tag.Id);
                    res.EnsureNoErrors();

                    project.Nodes = res.Data.DesCommentThreads
                        .OrderByDescending(x => x.ThreadNumber)
                        .Select(x => (TreeNode)new ThreadNode(x, project))
                        .ToHashSet();

                    return project.Nodes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        void ActivatedValueChanged(TreeNode node)
        {
            if (node is WorkspaceNode workspace)
            {
                AppData.CurrentWorkspace = workspace;
                NavManager.NavigateTo("workspace");
                return;
            }

            if (node is ProjectNode project)
            {
                AppData.CurrentProject = project;
                NavManager.NavigateTo("project");
                return;
            }

            if (node is ThreadNode thread)
            {
                AppData.CurrentThread = thread;
                NavManager.NavigateTo("thread");
                return;
            }
        }
    }
}
