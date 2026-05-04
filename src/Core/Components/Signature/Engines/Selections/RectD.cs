namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rectangle defined by its top-left corner (X, Y) and its dimensions (Width, Height) using double-precision floating-point values.
/// </summary>
/// <param name="X">X-coordinate of the top-left corner of the rectangle.</param>
/// <param name="Y">Y-coordinate of the top-left corner of the rectangle.</param>
/// <param name="Width">Width of the rectangle.</param>
/// <param name="Height">Height of the rectangle.</param>
public readonly record struct RectD(double X, double Y, double Width, double Height)
{
    /// <summary>
    /// Gets the horizontal position of the left edge.
    /// </summary>
    public double Left => X;

    /// <summary>
    /// Gets the vertical position of the object relative to its origin.
    /// </summary>
    public double Top => Y;

    /// <summary>
    /// Gets the x-coordinate of the right edge of the rectangle.
    /// </summary>
    public double Right => X + Width;

    /// <summary>
    /// Gets the y-coordinate of the bottom edge of the rectangle.
    /// </summary>
    public double Bottom => Y + Height;

    /// <summary>
    /// Determines whether the specified point is contained within the bounds of the rectangle.
    /// </summary>
    /// <param name="x">The x-coordinate of the point to test for containment.</param>
    /// <param name="y">The y-coordinate of the point to test for containment.</param>
    /// <returns><see langword="true"/> if the point defined by <paramref name="x"/> and <paramref name="y"/> is within the
    /// rectangle; otherwise, <see langword="false"/>.</returns>
    public bool Contains(double x, double y) => x >= Left && x <= Right && y >= Top && y <= Bottom;
}
