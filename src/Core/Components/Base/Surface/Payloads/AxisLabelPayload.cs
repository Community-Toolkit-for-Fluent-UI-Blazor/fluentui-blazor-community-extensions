using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Surface.Payloads;

/// <summary>
/// Represents the payload for an axis label, including its display text and position on a chart axis.
/// </summary>
public sealed class AxisLabelPayload
{
    /// <summary>
    /// Gets the text content associated with this instance.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Gets the position of the chart element in data coordinates.
    /// </summary>
    public required ChartPoint Position { get; init; }

    /// <summary>
    /// Gets the rotation angle of the label, in degrees.
    /// </summary>
    public double Rotation { get; init; }

    /// <summary>
    /// Gets the text anchor used to align the label horizontally.
    /// </summary>
    public SvgTextAnchor Anchor { get; init; } = SvgTextAnchor.Middle;

    /// <summary>
    /// Gets the font size of the label.
    /// </summary>
    public double FontSize { get; init; } = 12.0;
}
