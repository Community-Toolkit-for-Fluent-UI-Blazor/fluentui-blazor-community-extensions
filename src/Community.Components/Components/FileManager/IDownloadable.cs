namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface to allow a file to be downloaded or not.
/// </summary>
public interface IDownloadable
{
    /// <summary>
    /// Gets a value indicating if the file can be downloaded or not.
    /// </summary>
    bool IsDownloadAllowed { get; }
}
