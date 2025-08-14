namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the rendering mode for files in a chat component.
/// </summary>
public enum ChatFileRenderingMode
{
    /// <summary>
    /// The files are rendered as a FluentCounterBadge on the media button.
    /// </summary>
    Discrete,

    /// <summary>
    /// A viewer is used to render the files, allowing for a more detailed view of the file contents.
    /// </summary>
    Viewer
}
