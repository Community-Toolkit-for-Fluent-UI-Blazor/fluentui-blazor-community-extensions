namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for objects that support conditional renaming functionality.
/// </summary>
/// <remarks>Implement this interface to indicate that an object can be renamed, subject to the value of the
/// IsRenameAllowed property.</remarks>
public interface IRenamable
{
    /// <summary>
    /// Gets or sets a value indicating whether renaming is permitted.
    /// </summary>
    bool IsRenameAllowed { get; set; }
}
