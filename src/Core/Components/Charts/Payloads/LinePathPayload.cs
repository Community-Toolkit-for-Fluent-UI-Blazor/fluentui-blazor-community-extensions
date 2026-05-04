using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a line path layer in a chart, containing the data points and rendering options required
/// to display a line path.
/// </summary>
/// <remarks>This type is used to encapsulate the data and configuration for rendering a line path in charting
/// components. It includes a unique identifier, a collection of chart points that define the path, and a flag
/// indicating whether the path should be rendered smoothly. Instances of this class are typically used as part of chart
/// layer composition and can be compared for equality based on their content.</remarks>
public sealed class LinePathPayload : ILayerPayload, IEquatable<LinePathPayload>
{
    /// <summary>
    /// Gets the unique identifier for this instance.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the collection of data points to be displayed in the chart.
    /// </summary>
    /// <remarks>The order of points in the collection determines their sequence in the chart. The collection
    /// must not be null and should contain at least one point for the chart to render meaningful data.</remarks>
    public required IReadOnlyList<ChartPoint> Points { get; init; }

    /// <summary>
    /// Gets a value indicating whether smooth transitions or animations are enabled.
    /// </summary>
    public required bool Smooth { get; init; }

    /// <inheritdoc />
    public bool Equals(LinePathPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (Smooth != other.Smooth)
        {
            return false;
        }

        if (Points.Count != other.Points.Count)
        {
            return false;
        }

        for (var i = 0; i < Points.Count; i++)
        {
            if (Points[i] != other.Points[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not LinePathPayload other)
        {
            return false;
        }

        return Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Id, Smooth, Points.Count);
}
