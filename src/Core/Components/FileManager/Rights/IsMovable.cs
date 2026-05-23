namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for objects that support a deletable state.
/// </summary>
public interface IMovable
{
    /// <summary>
    /// Gets or sets a value indicating whether move operations are permitted.
    /// </summary>
    bool IsMoveAllowed { get; set; }
}
