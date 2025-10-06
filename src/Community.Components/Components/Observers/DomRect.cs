namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the dimensions and position of a rectangle, typically used to describe the bounding box of an element in
/// a coordinate space.
/// </summary>
/// <remarks>The rectangle is defined by its top, left, bottom, and right edges, as well as its width and height.
/// All values are expressed in the same coordinate system, and may be fractional. This type is commonly used in
/// scenarios such as layout calculations, collision detection, or graphical rendering.</remarks>
public record DomRect
{
    /// <summary>
    /// Gets the top coordinate value.
    /// </summary>
    public double Top { get; init; }

    /// <summary>
    /// Gets the left coordinate value.
    /// </summary>
    public double Left { get; init; }

    /// <summary>
    /// Gets the bottom coordinate value.
    /// </summary>
    public double Bottom { get; init; }

    /// <summary>
    /// Gets the right coordinate value.
    /// </summary>
    public double Right { get; init; }

    /// <summary>
    /// Gets the width of the element.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the element.
    /// </summary>
    public double Height { get; init; }
}
