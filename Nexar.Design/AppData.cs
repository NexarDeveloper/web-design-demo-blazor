using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexar.Design;

/// <summary>
/// The application parameters and state.
/// </summary>
public static class AppData
{
    /// <summary>
    /// The Nexar GraphQL API endpoint.
    /// </summary>
    public static string ApiEndpoint { get; private set; }

    /// <summary>
    /// The identity server endpoint.
    /// </summary>
    public static string Authority { get; private set; }

    /// <summary>
    /// The Nexar home page.
    /// </summary>
    public static string NexarDotCom { get; private set; }

    /// <summary>
    /// The local storage key for token.
    /// </summary>
    public static string KeyToken { get; private set; }

    /// <summary>
    /// The application mode.
    /// </summary>
    public static AppMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            Init();
        }
    }

    static AppMode _mode;

    static AppData()
    {
        Init();
    }

    static void Init()
    {
        switch (_mode)
        {
            case AppMode.Prod:
                ApiEndpoint = "https://api.nexar.com/graphql";
                Authority = "https://identity.nexar.com";
                NexarDotCom = "https://nexar.com";
                KeyToken = "_210528_p1";
                break;
            default:
                throw new Exception();
        }
    }

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
}

public enum AppMode
{
    Prod,
}
