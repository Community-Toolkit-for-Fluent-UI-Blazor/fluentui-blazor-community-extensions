using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a polar label in a polar chart.
/// </summary>
public sealed record PolarLabelPayload
{
    /// <summary>
    /// Gets the text of the label.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Gets the position of the label in polar coordinates.
    /// </summary>
    public required ChartPoint Position { get; init; }

    /// <summary>
    /// Gets the anchor point of the label text.
    /// </summary>
    public SvgTextAnchor Anchor { get; init; } = SvgTextAnchor.Middle;

    /// <summary>
    /// Gets the font size of the label text.
    /// </summary>
    public double FontSize { get; init; } = 12;

    /// <summary>
    /// Gets the rotation of the polar label.
    /// </summary>
    public double Rotation { get; init; }
}

