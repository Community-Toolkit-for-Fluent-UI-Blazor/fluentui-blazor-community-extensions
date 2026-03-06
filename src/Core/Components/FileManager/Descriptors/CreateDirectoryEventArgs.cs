namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when a new directory is being created.
/// </summary>
/// <remarks>This class supplies information about the parent directory and the name of the new directory to be
/// created. The caller can optionally provide a descriptor for the new directory by setting the Descriptor property
/// before the event is handled.</remarks>
public sealed class CreateDirectoryEventArgs
{
    /// <summary>
    /// Gets the identifier of the parent entity associated with this instance.
    /// </summary>
    public string ParentId { get; init; } = default!;

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Gets or sets the descriptor that provides metadata about the directory.
    /// </summary>
    public DirectoryDescriptor? Descriptor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation should be canceled.
    /// </summary>
    public bool Cancel { get; set; }
}

