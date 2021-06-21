using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexar.Design.Layouts
{
    public partial class MainLayout : IDisposable
    {
        [CascadingParameter]
        Theme Theme { get; init; }

        [Inject]
        IJSRuntime JS { get; init; }

        [Inject]
        NavigationManager NavManager { get; init; }

        [Inject]
        AppData AppData { get; init; }

        [Inject]
        NexarClient Client { get; init; }

        Bar sidebar;
        bool topbarVisible = false;
        bool _showSlider;
        int _width1, _width2;

        protected override async Task OnInitializedAsync()
        {
            AppData.OnChange += StateHasChanged;

            _width2 = await JS.InvokeAsync<int>("getWindowWidth");
            _width1 = int.Parse(Theme.BarOptions.VerticalWidth.Replace("px", ""));
        }

        async Task SliderValueChanged(int value)
        {
            var coeff = (double)value / _width2;
            _width2 = await JS.InvokeAsync<int>("getWindowWidth");
            _width1 = Math.Max(300, (int)(_width2 * coeff));
            Theme.BarOptions.VerticalWidth = $"{_width1}px";
            Theme.ThemeHasChanged();
        }

        public void Dispose()
        {
            AppData.OnChange -= StateHasChanged;
            GC.SuppressFinalize(this);
        }

        void ResizeClicked()
        {
            _showSlider = !_showSlider;
        }

        async Task ExpandedNodesChanged(IList<TreeNode> nodes)
        {
            try
            {
                foreach (var node in nodes)
                {
                    if (node is WorkspaceNode workspace && workspace.Nodes.Count == 1 && workspace.Nodes[0].Name == null)
                    {
                        var res = await Client.Projects.ExecuteAsync(workspace.Tag.Url);
                        res.EnsureNoErrors();

                        workspace.Nodes = res.Data.DesProjects.Nodes.Select(x => new ProjectNode(x, workspace)).ToArray();
                        continue;
                    }

                    if (node is ProjectNode project && project.Nodes.Count == 1 && project.Nodes[0].Name == null)
                    {
                        var res = await Client.CommentThreads.ExecuteAsync(project.Tag.Id);
                        res.EnsureNoErrors();

                        project.Nodes = res.Data.DesCommentThreads
                            .OrderByDescending(x => x.ThreadNumber)
                            .Select(x => new ThreadNode(x, project))
                            .ToArray();
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void SelectedNodeChanged(TreeNode node)
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
