namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface to allow a file to be renamed or not.
/// </summary>
public interface IRenamable
{
    /// <summary>
    /// Gets a value indicating if the file can be renamed or not.
    /// </summary>
    bool IsRenamable { get; }
}

