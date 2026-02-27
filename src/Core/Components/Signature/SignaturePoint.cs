namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single point in a digital signature or drawing, including position, pressure, velocity, width, and
/// timestamp information.
/// </summary>
/// <remarks>This record is useful for capturing detailed information about each point in a signature or drawing,
/// enabling advanced rendering and analysis such as pressure-sensitive strokes or velocity-based effects.</remarks>
/// <param name="x">The X-coordinate of the point, typically measured in pixels or device units.</param>
/// <param name="y">The Y-coordinate of the point, typically measured in pixels or device units.</param>
/// <param name="pressure">The pressure applied at the point, usually normalized between 0 and 1, where higher values indicate greater
/// pressure.</param>
/// <param name="velocity">The velocity of the pen or pointer at the point, typically measured in units per second.</param>
/// <param name="width">The width of the stroke at the point, often used to represent the thickness of the drawn line.</param>
/// <param name="timestamp">The timestamp of the point, representing the time at which the point was recorded, typically in milliseconds.</param>
public sealed class SignaturePoint(
    double x,
    double y,
    double pressure = 1.0,
    double velocity = 0.0,
    double width = 1.0,
    double timestamp = 0.0)
{
    /// <summary>
    /// Gets or sets the X-coordinate of the point.
    /// </summary>
    public double X { get; set; } = x;

    /// <summary>
    /// Gets or sets the Y-coordinate of the point.
    /// </summary>
    public double Y { get; set; } = y;

    /// <summary>
    /// Gets or sets the pressure applied at the point.
    /// </summary>
    public double Pressure { get; set; } = pressure;

    /// <summary>
    /// Gets or sets the velocity of the pen or pointer at the point.
    /// </summary>
    public double Velocity { get; set; } = velocity;

    /// <summary>
    /// Gets or sets the width of the stroke at the point.
    /// </summary>
    public double Width { get; set; } = width;

    /// <summary>
    /// Gets or sets the timestamp of the point, representing the time at which the point was recorded.
    /// </summary>
    public double Timestamp { get; set; } = timestamp;

    /// <summary>
    /// Clones the current <see cref="SignaturePoint"/>, creating a new instance with the same property values.
    /// </summary>
    /// <returns>Returns the cloned <see cref="SignaturePoint"/>.</returns>
    public SignaturePoint Clone()
    {
        return new SignaturePoint(X, Y, Pressure, Velocity, Width, Timestamp);
    }
}
