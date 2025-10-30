namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that reports the new dimensions of a resized window.
/// </summary>
public record ResizeWindowEventArgs
{
    /// <summary>
    /// Gets the width of the window.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the window.
    /// </summary>
    public double Height { get; init; }
}
