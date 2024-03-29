using Nexar.Client;
using System;
using System.Collections.Generic;

namespace Nexar.Demo;

/// <summary>
/// The application parameters and state.
/// </summary>
public static class AppData
{
    /// <summary>
    /// The Nexar GraphQL API endpoint.
    /// </summary>
    public static AppMode Mode { get; private set; }

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
    /// Sets the application mode and optional custom service.
    /// </summary>
    public static void Initialize(AppMode mode, string apiEndpoint)
    {
        Mode = mode;

        if (!string.IsNullOrEmpty(apiEndpoint))
            ApiEndpoint = apiEndpoint;

        switch (Mode)
        {
            case AppMode.Prod:
                ApiEndpoint ??= "https://api.nexar.com/graphql";
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
        TreeItems = [new SharedWithMeItem()];

        foreach (var it in source)
            TreeItems.Add(new WorkspaceItem(it));

        OnChange?.Invoke();
    }
}

public enum AppMode
{
    Prod,
}
