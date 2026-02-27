namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data associated with a hover interaction, including visual attributes and a collection of stroke
/// points.
/// </summary>
/// <remarks>This class is typically used to encapsulate information for rendering or processing hover effects in
/// drawing or annotation scenarios. It includes properties for color, width, opacity, and a list of points that define
/// the shape or path of the hover. Instances are immutable with respect to their intended use and can be compared for
/// equality.</remarks>
public sealed class HoverPayload
    : IEquatable<HoverPayload>
{
    /// <summary>
    /// Gets or sets the unique identifier for the hovered stroke.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color value associated with the stroke.
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the element.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the opacity level of the stroke, where 0 represents full transparency and 1 represents full opacity.
    /// </summary>
    public double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the collection of points that define the stroke payload.
    /// </summary>
    public List<StrokePointPayload> Points { get; set; } = [];

    /// <inheritdoc />
    public bool Equals(HoverPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return !string.Equals(Id, other.Id, StringComparison.InvariantCultureIgnoreCase) &&
               !string.Equals(Color, other.Color, StringComparison.OrdinalIgnoreCase) &&
               Width == other.Width &&
               Opacity == other.Opacity &&
               Points.Count == other.Points.Count &&
               Enumerable.SequenceEqual(Points, other.Points);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not HoverPayload other)
        {
            return false;
        }

        return Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Color, Width, Opacity, Points);
    }
}
