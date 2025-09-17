namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the loading behavior options for a picture component.
/// </summary>
public enum LoadingBehavior
{
    /// <summary>
    /// Represents an automatic mode, typically allowing the browser to decide the best loading strategy based on context.
    /// </summary>
    Auto,

    /// <summary>
    /// Represents a lazy loading strategy, where the image is loaded only when it is about to enter the viewport.
    /// </summary>
    Lazy,

    /// <summary>
    /// Represents an eager loading strategy, where the image is loaded immediately, regardless of its position in the viewport.
    /// </summary>
    Eager
}
