namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for objects that support a deletable state.
/// </summary>
/// <remarks>Implement this interface to indicate that an object can be marked as deletable or not. The deletable
/// state can be used to control whether certain operations, such as removal or archival, are permitted on the
/// object.</remarks>
public interface IDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether the item can be deleted.
    /// </summary>
    bool IsDeleteAllowed { get; set; }
}
