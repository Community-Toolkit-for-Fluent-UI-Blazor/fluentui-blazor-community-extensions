using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents the configuration and properties of an axis in a chart.
/// </summary>
internal class ChartAxis
{
    /// <summary>
    /// Gets the type of the axis, which determines how data is interpreted and displayed along the axis.
    /// </summary>
    public ChartAxisType AxisType { get; set; }

    /// <summary>
    /// Gets the minimum value of the axis. This value defines the lower bound of the axis range and is used to determine the scaling and layout of the chart.
    /// </summary>
    public double Minimum { get; set; }

    /// <summary>
    /// Gets the maximum value of the axis. This value defines the upper bound of the axis range and is used to determine the scaling and layout of the chart.
    /// </summary>
    public double Maximum { get; set; }

    /// <summary>
    /// Gets a value indicating whether the axis is inverted.
    /// </summary>
    public bool IsInverted { get; set; }

    /// <summary>
    /// Gets the map function  to convert an index, a value to a position on the axis.
    /// </summary>
    public Func<double, double> Map { get; set; } = i => i;

    /// <summary>
    /// Gets or sets the rotation angle, in degrees, applied to the label text.
    /// </summary>
    public double LabelRotation { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of the labels on the axis.
    /// </summary>
    public double LabelWidth { get; set; }

    /// <summary>
    /// Gets or sets the maximum height of the label on the axis.
    /// </summary>
    public double LabelHeight { get; set; }

    /// <summary>
    /// Gets the list of category names associated with the item.
    /// </summary>
    public IReadOnlyList<string>? Labels { get; set; }
}
