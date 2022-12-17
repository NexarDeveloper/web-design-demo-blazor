using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexar.Design;

/// <summary>
/// The application state and options. Different modes and URLs are only
/// needed for internal development. Clients normally use nexar.com URLs.
/// </summary>
public static class AppData
{
    public static AppMode Mode { get; set; } = AppMode.Prod;

    /// <summary>
    /// The identity server endpoint.
    /// </summary>
    public static string Authority =>
        Mode switch
        {
            AppMode.Prod => "https://identity.nexar.com/",
            AppMode.Dev => "https://identity.nexar.com/",
            _ => "https://identity.nexar.com/",
        };

    /// <summary>
    /// The Nexar GraphQL API endpoint.
    /// </summary>
    public static string ApiEndpoint =>
        Mode switch
        {
            AppMode.Prod => "https://api.nexar.com/graphql",
            AppMode.Dev => "https://api.nexar.com/graphql",
            _ => "https://api.nexar.com/graphql",
        };

    /// <summary>
    /// The Nexar home page.
    /// </summary>
    public static string NexarDotCom =>
        Mode switch
        {
            AppMode.Prod => "https://nexar.com",
            AppMode.Dev => "https://nexar.com",
            _ => "https://nexar.com",
        };

    /// <summary>
    /// The local storage key for token.
    /// </summary>
    public static string KeyToken =>
        Mode switch
        {
            AppMode.Prod => "_210528_p1",
            AppMode.Dev => "_210529_9a",
            _ => "_220225_iz",
        };

    /// <summary>
    /// This event is triggered on changes.
    /// </summary>
    public static event Action OnChange;

    public static void Reset()
    {
        NexarClientFactory.AccessToken = null;
        TreeItems = null;
        OnChange?.Invoke();
    }

    public static HashSet<TreeItem> TreeItems { get; private set; }
    public static void SetWorkspaces(IReadOnlyList<IMyWorkspace> source)
    {
        TreeItems = source.Select(x => (TreeItem)new WorkspaceItem(x)).ToHashSet();
        OnChange?.Invoke();
    }

    static WorkspaceItem currentWorkspace;
    public static event Action OnChangeWorkspace;
    public static WorkspaceItem CurrentWorkspace
    {
        get => currentWorkspace;
        set
        {
            currentWorkspace = value;
            OnChangeWorkspace?.Invoke();
        }
    }

    static ProjectItem currentProject;
    public static event Action OnChangeProject;
    public static ProjectItem CurrentProject
    {
        get => currentProject;
        set
        {
            currentProject = value;
            OnChangeProject?.Invoke();
        }
    }

    static ThreadItem currentThread;

    public static event Action OnChangeThread;
    public static ThreadItem CurrentThread
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
    Uat,
}
