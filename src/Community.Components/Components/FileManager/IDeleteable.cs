namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface to allow a file to be deleted or not.
/// </summary>
public interface IDeletable
{
    /// <summary>
    /// Gets a value indicating if the file can be deleted or not.
    /// </summary>
    bool IsDeleteable { get; }
}
