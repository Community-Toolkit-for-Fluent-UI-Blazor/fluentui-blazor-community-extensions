namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the axes payload.
/// </summary>
public sealed class AxesPayload : IEquatable<AxesPayload>
{
    /// <summary>
    /// Gets or sets the color value in hexadecimal format.
    /// </summary>
    public string Color { get; init; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component, where 1.0 is fully opaque and
    /// 0.0 is fully transparent. Values outside the range of 0.0 to 1.0 may not be supported and could result in
    /// undefined behavior.</remarks>
    public double Opacity { get; init; } = 1.0;

    /// <summary>
    /// Gets or sets the width of the stroke used to render the component.
    /// </summary>
    public double StrokeWidth { get; init; } = 2.0;

    /// <summary>
    /// Gets or sets the dash pattern used to render the outline of a shape or path.
    /// </summary>
    /// <remarks>The dash pattern is specified as a string of comma-separated numbers, where each number
    /// represents the length of dashes and gaps in the pattern. For example, "5,2" creates a pattern of a 5-unit dash
    /// followed by a 2-unit gap. If the value is null or empty, a solid line is rendered.</remarks>
    public double[] DashArray { get; init; } = [];

    /// <summary>
    /// Gets or sets the grid layer on which the component is rendered.
    /// </summary>
    /// <remarks>Use this property to control whether the component appears above or below other grid
    /// elements, such as strokes or content layers. The default value is <see
    /// cref="GridLayer.Background"/>.</remarks>
    public GridLayer Layer { get; init; } = GridLayer.Background;

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Color, StringComparer.OrdinalIgnoreCase);
        hash.Add(Opacity);
        hash.Add(StrokeWidth);
        hash.Add(Layer);

        foreach (var d in DashArray)
        {
            hash.Add(d);
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is AxesPayload other)
        {
            return Equals(this, other);
        }

        return false;
    }

    /// <inheritdoc />
    public bool Equals(AxesPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Color, other.Color, StringComparison.OrdinalIgnoreCase) &&
               Opacity == other.Opacity &&
               StrokeWidth == other.StrokeWidth &&
               Layer == other.Layer &&
               DashArray.Length == other.DashArray.Length &&
               DashArray.SequenceEqual(other.DashArray);
    }
}
