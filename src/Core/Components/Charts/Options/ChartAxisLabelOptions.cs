using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents configuration options for axis label appearance in a chart.
/// </summary>
/// <remarks>Use this class to specify visual properties such as font size, rotation angle, and anchor alignment
/// for axis labels when rendering charts. These options allow customization of label readability and positioning to
/// suit different chart layouts.</remarks>
public sealed class ChartAxisLabelOptions
{
    /// <summary>
    /// Gets the font size to use for rendering text.
    /// </summary>
    public double FontSize { get; init; } = 12.0;

    /// <summary>
    /// Gets the rotation angle, in degrees, applied to the element.
    /// </summary>
    public double Rotation { get; init; }

    /// <summary>
    /// Gets the anchor position used for aligning content.
    /// </summary>
    public SvgTextAnchor Anchor { get; init; } = SvgTextAnchor.Middle;
}
