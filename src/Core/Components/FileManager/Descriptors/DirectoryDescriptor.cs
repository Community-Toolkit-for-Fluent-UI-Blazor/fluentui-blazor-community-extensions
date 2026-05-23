namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a descriptor for a directory, including its unique identifier and display name.
/// </summary>
/// <param name="Id">The unique identifier for the directory. Cannot be null.</param>
public sealed record DirectoryDescriptor(string Id)
{
    /// <summary>
    /// Gets or sets the name associated with this instance.
    /// </summary>
    internal string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the parent entity associated with this instance.
    /// </summary>
    internal string ParentId { get; set; } = default!;
}
