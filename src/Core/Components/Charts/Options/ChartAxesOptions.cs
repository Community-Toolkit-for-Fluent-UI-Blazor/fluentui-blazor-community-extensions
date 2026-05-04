namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents configuration options for customizing the appearance and labeling of an axis in a chart or data
/// visualization.
/// </summary>
/// <remarks>Use this class to specify which axis elements are visible, how ticks and labels are rendered, and to
/// provide custom formatting or labeling logic. All properties are immutable and must be set during object
/// initialization.</remarks>
internal sealed class ChartAxisOptions : SurfaceAxesOptions
{
    /// <summary>
    /// Gets a value indicating whether tick marks are displayed on the slider control.
    /// </summary>
    public bool ShowTicks { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the associated elements.
    /// </summary>
    public bool ShowLabels { get; init; } = true;

    /// <summary>
    /// Gets the length of each tick mark, in device-independent units (DIPs).
    /// </summary>
    public double TickLength { get; init; } = 4.0;

    /// <summary>
    /// Gets the maximum number of ticks allowed for the operation.
    /// </summary>
    public int MaxTicks { get; init; } = 6;

    /// <summary>
    /// Gets the offset, in pixels, applied to the label position relative to its default location.
    /// </summary>
    public double LabelOffset { get; init; } = 12.0;

    /// <summary>
    /// Gets the standard or custom numeric format string used to format numeric values.
    /// </summary>
    /// <remarks>The format string determines how numeric values are converted to their string representation.
    /// The default value is "G", which specifies the general numeric format. For more information about valid format
    /// strings, see Standard Numeric Format Strings and Custom Numeric Format Strings in the .NET
    /// documentation.</remarks>
    public string NumericFormat { get; init; } = "G";

    /// <summary>
    /// Gets the options used to configure the appearance and behavior of axis labels on the chart.
    /// </summary>
    public ChartAxisLabelOptions? LabelOptions { get; init; }
}
