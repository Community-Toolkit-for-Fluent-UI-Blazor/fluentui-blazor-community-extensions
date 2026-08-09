namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of motion gesture variants that can be applied to UI components to define their interactive
/// behaviors.
/// </summary>
/// <remarks>Use this class to specify different motion variants for common user interactions such as hover,
/// press, tap, and drag. Each property corresponds to a specific gesture and can be set independently to customize the
/// motion response for that gesture.</remarks>
public sealed class MotionGestures
{
    /// <summary>
    /// Gets or sets the motion variant to apply when the component is hovered.
    /// </summary>
    public MotionVariant? Hover { get; set; }

    /// <summary>
    /// Gets or sets the motion variant to use when the press interaction is triggered.
    /// </summary>
    public MotionVariant? Press { get; set; }

    /// <summary>
    /// Gets or sets the motion variant to apply when the element is tapped.
    /// </summary>
    public MotionVariant? Tap { get; set; }

    /// <summary>
    /// Gets or sets the motion variant to apply when the component is dragged.
    /// </summary>
    public MotionVariant? Drag { get; set; }
}

