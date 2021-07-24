using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexar.Design
{
    /// <summary>
    /// The application state and options. Different modes and URLs are only
    /// needed for internal development. Clients normally use nexar.com URLs.
    /// </summary>
    public class AppData
    {
        public string Token { get; set; }
        public AppMode Mode { get; set; } = AppMode.Prod;

        /// <summary>
        /// The identity server endpoint.
        /// </summary>
        public string Authority =>
            Mode == AppMode.Prod
            ? "https://identity.nexar.com/"
            : "https://identity.nexar.com/";

        /// <summary>
        /// The Nexar GraphQL API endpoint.
        /// </summary>
        public string ApiEndpoint =>
            Mode == AppMode.Prod
            ? "https://api.nexar.com/graphql"
            : "https://api.nexar.com/graphql";

        /// <summary>
        /// The Nexar home page.
        /// </summary>
        public string NexarDotCom =>
            Mode == AppMode.Prod
            ? "http://nexar.com"
            : "http://nexar.com";

        /// <summary>
        /// The local storage key for token.
        /// </summary>
        public string KeyToken =>
            Mode == AppMode.Prod
            ? "_210528_p1"
            : "_210529_9a";

        /// <summary>
        /// This event is triggered on changes.
        /// </summary>
        public event Action OnChange;

        public void Reset()
        {
            Token = null;
            TreeItems = null;
            OnChange?.Invoke();
        }

        public HashSet<TreeItem> TreeItems { get; private set; }
        public void SetWorkspaces(IReadOnlyList<IMyWorkspace> source)
        {
            TreeItems = source.Select(x => (TreeItem)new WorkspaceItem(x)).ToHashSet();
            OnChange?.Invoke();
        }

        WorkspaceItem currentWorkspace;
        public event Action OnChangeWorkspace;
        public WorkspaceItem CurrentWorkspace
        {
            get => currentWorkspace;
            set
            {
                currentWorkspace = value;
                OnChangeWorkspace?.Invoke();
            }
        }

        ProjectItem currentProject;
        public event Action OnChangeProject;
        public ProjectItem CurrentProject
        {
            get => currentProject;
            set
            {
                currentProject = value;
                OnChangeProject?.Invoke();
            }
        }

        ThreadItem currentThread;
        public event Action OnChangeThread;
        public ThreadItem CurrentThread
        {
            get => currentThread;
            set
            {
                currentThread = value;
                OnChangeThread?.Invoke();
            }
        }
    }

    public enum AppMode
    {
        Prod,
        Dev,
    }
}
