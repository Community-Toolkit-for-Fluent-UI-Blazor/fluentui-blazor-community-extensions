namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a strategy for laying out animated elements within a container.
/// </summary>
public interface ILayoutStrategy
    : IDisposable
{
    /// <summary>
    /// Gets or sets a value indicating whether the layout changes should be applied immediately without animation.
    /// </summary>
    bool Immediate { get; set; }

    /// <summary>
    /// Gets or sets the duration of the layout animation.
    /// </summary>
    TimeSpan Duration { get; set; }

    /// <summary>
    /// Applies the layout strategy to the provided list of animated elements.
    /// </summary>
    /// <param name="elements">Elements to animate within this layout instance.</param>
    void ApplyLayout(List<AnimatedElement> elements);

    /// <summary>
    /// Applies the start time for the layout animation.
    /// </summary>
    /// <param name="now">Start time of the layout animation.</param>
    void ApplyStartTime(DateTime now);

    /// <summary>
    /// Gets or sets the easing function used for the layout animation.
    /// </summary>
    EasingFunction EasingFunction { get; set; }

    /// <summary>
    /// Gets or sets the easing mode used for the layout animation.
    /// </summary>
    EasingMode EasingMode { get; set; }

    /// <summary>
    /// Sets the dimensions of the container in which the layout is applied.
    /// </summary>
    /// <param name="width">Width of the container.</param>
    /// <param name="height">Height of the container.</param>
    void SetDimensions(double width, double height);
}
