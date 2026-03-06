namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the folder sorting mode of the files.
/// </summary>
public enum FileSortLayout
{
    /// <summary>
    /// Specifies that folders and files are listed together without any specific ordering.
    /// </summary>
    None,

    /// <summary>
    /// Specifies that directories are listed before files when displaying or
    ///  enumerating file system entries.
    /// </summary>
    Folders,

    /// <summary>
    /// Specifies that files are listed before directories when displaying or
    ///  enumerating file system entries.
    /// </summary>
    Files,
}
