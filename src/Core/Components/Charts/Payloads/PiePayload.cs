using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a pie chart item.
/// </summary>
public sealed record PiePayload : ChartItemPayloadBase, ILayerPayload, IEquatable<PiePayload>
{
    /// <summary>
    /// Gets the starting angle, in degrees, for the component's rendering or calculation.
    /// </summary>
    /// <remarks>The start angle determines where the drawing or measurement begins, typically measured from
    /// the positive X-axis in a counterclockwise direction. Adjust this value to control the initial orientation of the
    /// component.</remarks>
    public required double StartAngle { get; init; }

    /// <summary>
    /// Gets the end angle, in degrees, for the arc or segment.
    /// </summary>
    /// <remarks>The end angle is typically measured in degrees, where 0 degrees represents the positive
    /// X-axis. The interpretation of the angle may depend on the coordinate system and usage context.</remarks>
    public required double EndAngle { get; init; }

    /// <summary>
    /// Gets the mid angle, in degrees, representing the central angle of the slice.
    /// </summary>
    public required double MidAngle { get; init; }

    /// <summary>
    /// Gets the optional label for the slice.
    /// </summary>
    public double Value { get; init; }

    /// <summary>
    /// Gets the label text of the chart item.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Gets the position of the label relative to the chart point, if specified.
    /// </summary>
    public ChartPoint? LabelPosition { get; init; }

    /// <summary>
    /// Gets the radius of the pie slice.
    /// </summary>
    public double Radius { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the pie slice should use alternate animation.
    /// </summary>
    public bool AlternateAnimation { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this slice is part of a multi-donut chart.
    /// </summary>
    public required bool IsMultiDonutSlice { get; init; }

    /// <summary>
    /// Gets the index of the color used for this slice.
    /// </summary>
    public int ColorIndex { get; internal set; }

    /// <inheritdoc />
    public bool Equals(PiePayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase) &&
               !string.Equals(Label, other.Label, StringComparison.OrdinalIgnoreCase) &&
               Value == other.Value &&
               StartAngle == other.StartAngle &&
               EndAngle == other.EndAngle;
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Id, Value, Label, StartAngle, EndAngle);
}
