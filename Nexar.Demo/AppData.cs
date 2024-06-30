using MudBlazor;
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
    /// The Nexar home page.
    /// </summary>
    public static string NexarDotCom { get; private set; }

    public static bool IsRegionApi { get; private set; }

    /// <summary>
    /// Sets the application mode and optional custom service.
    /// </summary>
    public static void Initialize(AppMode mode, string apiEndpoint)
    {
        Mode = mode;

        if (!string.IsNullOrEmpty(apiEndpoint))
        {
            ApiEndpoint = apiEndpoint;
            IsRegionApi = true;
        }

        switch (Mode)
        {
            case AppMode.Prod:
                ApiEndpoint ??= "https://api.nexar.com/graphql";
                NexarDotCom = "https://nexar.com";
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

    public static List<TreeItemData<TreeItem>> TreeItems { get; private set; }
    public static void SetWorkspaces(IReadOnlyList<IMyWorkspace> source, string workspaceAuthId)
    {
        TreeItems = [new MyTreeItemData(new SharedWithMeItem())];

        // add workspaces depending on auth
        if (workspaceAuthId is null)
        {
            // all workspaces allowed
            foreach (var it in source)
                TreeItems.Add(new MyTreeItemData(new WorkspaceItem(it, true)));
        }
        else
        {
            // the allowed workspace
            foreach (var it in source)
            {
                if (string.Equals(it.AuthId, workspaceAuthId, StringComparison.OrdinalIgnoreCase))
                {
                    TreeItems.Add(new MyTreeItemData(new WorkspaceItem(it, true)));
                    break;
                }
            }

            // other workspaces
            foreach (var it in source)
            {
                if (!string.Equals(it.AuthId, workspaceAuthId, StringComparison.OrdinalIgnoreCase))
                    TreeItems.Add(new MyTreeItemData(new WorkspaceItem(it, false)));
            }
        }

        OnChange?.Invoke();
    }
}

public enum AppMode
{
    Prod,
}
