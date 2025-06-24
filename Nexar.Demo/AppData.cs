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

    public static void SetWorkspaces(IReadOnlyList<IMyWorkspace> source, string? workspaceAuthId, string? workspaceUrl)
    {
        TreeItems = workspaceUrl is null ? [new MyTreeItemData(new SharedWithMeItem())] : [];

        // add workspaces depending on input
        if (workspaceUrl is { })
        {
            workspaceUrl = new Uri(workspaceUrl).AbsoluteUri;
            foreach (var it in source)
            {
                if (it.Url == workspaceUrl)
                {
                    var workspaceItem = new WorkspaceItem(it, true);
                    TreeItems = [.. workspaceItem.CreateChildItems().Select(x => (TreeItemData<TreeItem>)new MyTreeItemData(x))];
                    break;
                }
            }
        }
        else if (workspaceAuthId is null)
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
