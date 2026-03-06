namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the possible states of a file operation progress indicator.
/// </summary>
/// <remarks>Use this enumeration to represent the current progress state of file-related operations such
/// as uploading, downloading, deleting, moving, creating, or renaming. This can be used to update UI elements or
/// trigger logic based on the operation in progress.</remarks>
internal enum FileManagerProgressState
{
    /// <summary>
    /// Represents the state when no file operation is currently in progress.
    ///  This is the default state when the file manager is idle.
    /// </summary>
    None,

    /// <summary>
    /// Represents the current uploading state.
    /// </summary>
    Uploading,

    /// <summary>
    /// Represents the current downloading state.
    /// </summary>
    Downloading,

    /// <summary>
    /// Represents a state indicating that a delete operation is in progress.
    /// </summary>
    Deleting,

    /// <summary>
    /// Represents a state indicating that a move operation is in progress.
    /// </summary>
    Moving,

    /// <summary>
    /// Represents a state indicating that a create operation is in progress.
    /// </summary>
    Creation,

    /// <summary>
    /// Represents a state indicating that a rename operation is in progress.
    /// </summary>
    Renaming,

    /// <summary>
    /// Represents a state indicating that a search operation is in progress.
    /// </summary>
    Searching
}
