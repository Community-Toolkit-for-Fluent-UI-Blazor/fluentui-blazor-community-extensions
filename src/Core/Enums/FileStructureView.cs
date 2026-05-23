namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents how the file structure is displayed in the
/// <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public enum FileStructureView
{
    /// <summary>
    /// Displays the file system in a hierarchical structure.
    /// </summary>
    /// <remarks>
    /// Folders and subfolders are shown as a tree. This mode is ideal for
    /// navigating deep directory structures.
    /// </remarks>
    Hierarchical,

    /// <summary>
    /// Displays the file system as a flat list.
    /// </summary>
    /// <remarks>
    /// All files are shown in a single level, regardless of their folder.
    /// Useful for global search results or simplified browsing.
    /// </remarks>
    Flat
}
