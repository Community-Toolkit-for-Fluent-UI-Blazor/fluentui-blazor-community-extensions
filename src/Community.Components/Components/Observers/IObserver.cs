namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an observer that responds to asynchronous events related to resizing, intersection, and measurement.
/// </summary>
/// <remarks>This interface defines methods for handling specific asynchronous events, such as changes in size,
/// intersection events,  and measurement updates. Implementations of this interface are expected to handle these events
/// appropriately and  support asynchronous disposal.</remarks>
public interface IObserver
    : IAsyncDisposable
{
    /// <summary>
    /// Gets the unique identifier for the observer.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Handles the event triggered when the size of a component or window is resized.
    /// </summary>
    /// <param name="width">The new width, in pixels, after the resize event.</param>
    /// <param name="height">The new height, in pixels, after the resize event.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask OnResizedAsync(int width, int height);

    /// <summary>
    /// Handles the intersection event triggered when an observed element intersects with a specified threshold.
    /// </summary>
    /// <param name="e">The event arguments containing details about the intersection, such as the target element and intersection
    /// ratio. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask OnIntersectAsync(IntersectionEventArgs e);

    /// <summary>
    /// Handles the event triggered when a measurement is completed, providing the measured width and height.
    /// </summary>
    /// <param name="width">The measured width, in pixels. Must be a non-negative value.</param>
    /// <param name="height">The measured height, in pixels. Must be a non-negative value.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask OnMeasuredAsync(int width, int height);
}
