using MudBlazor;
using Nexar.Client;

namespace Nexar.Demo;

/// <summary>
/// Application parameters and state.
/// </summary>
public static class AppData
{
    /// <summary>
    /// Nexar mode.
    /// </summary>
    public static AppMode Mode { get; private set; }

    /// <summary>
    /// Nexar GraphQL API endpoint.
    /// </summary>
    public static string ApiEndpoint { get; private set; } = null!;

    /// <summary>
    /// Pinned workspace URL.
    /// </summary>
    public static string? WorkspaceUrl { get; private set; }

    /// <summary>
    /// Nexar home page.
    /// </summary>
    public static string NexarDotCom { get; private set; } = null!;

    public static string VoyagerEndpoint => ApiEndpoint.TrimEnd('/')[0..^7] + "ui/voyager";

    public static bool IsRegionApi { get; private set; }

    /// <summary>
    /// Sets the application mode and optional custom service.
    /// </summary>
    public static void Initialize(AppMode mode, string? apiEndpoint, string? workspaceUrl)
    {
        Mode = mode;
        WorkspaceUrl = workspaceUrl;

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
                throw new InvalidOperationException($"Invalid mode: '{Mode}'.");
        }
    }

    /// <summary>
    /// This event is triggered on changes.
    /// </summary>
    public static event Action? OnChange;

    public static void Reset()
    {
        TreeItems = null;
        OnChange?.Invoke();
    }

    public static List<TreeItemData<TreeItem>>? TreeItems { get; private set; }

    public static void SetWorkspaces(IReadOnlyList<IMyWorkspace> source, string? workspaceAuthId)
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

    public static void SetWorkspace(string workspaceUrl)
    {
        var workspaceInfo = new Workspaces_DesWorkspaceInfos_DesWorkspaceInfo(
            workspaceId: string.Empty,
            authId: string.Empty,
            url: workspaceUrl,
            name: string.Empty,
            description: null,
            location: null!);

        var workspaceItem = new WorkspaceItem(workspaceInfo, true);

        TreeItems = workspaceItem.CreateChildItems()
            .Select(x => (TreeItemData<TreeItem>)new MyTreeItemData(x))
            .ToList();

        OnChange?.Invoke();
    }
}

public enum AppMode
{
    Prod,
}
