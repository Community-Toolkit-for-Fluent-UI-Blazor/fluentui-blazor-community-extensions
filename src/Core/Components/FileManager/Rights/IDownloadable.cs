namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for objects that support download functionality.
/// </summary>
public interface IDownloadable
{
    /// <summary>
    /// Gets or sets a value indicating whether the item can be downloaded.
    /// </summary>
    bool IsDownloadAllowed { get; set; }
}
