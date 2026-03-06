namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines the set of capabilities supported by a file provider, indicating which file system operations are available.
/// </summary>
/// <remarks>Implementations use these properties to communicate supported operations such as deleting, renaming,
/// moving, creating directories, and uploading files. Consumers should check these properties before attempting an
/// operation to ensure it is supported by the underlying provider.</remarks>
public interface IFileProviderCapabilities
{
    /// <summary>
    /// Gets a value indicating whether the current item can be deleted.
    /// </summary>
    bool CanDelete { get; }

    /// <summary>
    /// Gets a value indicating whether the item can be renamed.
    /// </summary>
    bool CanRename { get; }

    /// <summary>
    /// Gets a value indicating whether the current item can be moved.
    /// </summary>
    bool CanMove { get; }

    /// <summary>
    /// Gets a value indicating whether a new directory can be created in the current context.
    /// </summary>
    bool CanCreateDirectory { get; }

    /// <summary>
    /// Gets a value indicating whether uploading is permitted.
    /// </summary>
    bool CanUpload { get; }
}

