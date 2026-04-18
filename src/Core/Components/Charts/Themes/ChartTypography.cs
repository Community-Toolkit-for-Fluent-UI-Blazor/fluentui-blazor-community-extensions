namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the typography settings for a chart, including styles for various text elements such as title, subtitle,
///  axis, legend, labels, and values.
/// </summary>
public sealed record ChartTypography
{
    /// <summary>
    /// Gets the text style applied to the chart title.
    /// </summary>
    public ChartTextStyle Title { get; init; } = ChartTextStyle.DefaultTitle;

    /// <summary>
    /// Gets the text style applied to the chart subtitle.
    /// </summary>
    public ChartTextStyle Subtitle { get; init; } = ChartTextStyle.DefaultSubtitle;

    /// <summary>
    /// Gets the text style applied to axis labels in the chart.
    /// </summary>
    public ChartTextStyle Axis { get; init; } = ChartTextStyle.DefaultAxis;

    /// <summary>
    /// Gets the text style applied to the chart legend.
    /// </summary>
    public ChartTextStyle Legend { get; init; } = ChartTextStyle.DefaultLegend;

    /// <summary>
    /// Gets the text style applied to the chart label.
    /// </summary>
    public ChartTextStyle Label { get; init; } = ChartTextStyle.DefaultLabel;

    /// <summary>
    /// Gets the text style applied to the chart values.
    /// </summary>
    public ChartTextStyle Value { get; init; } = ChartTextStyle.DefaultValue;

    /// <summary>
    /// Gets the text style used for small labels in the chart.
    /// </summary>
    public ChartTextStyle SmallLabel { get; init; } = ChartTextStyle.SmallLabel;

    /// <summary>
    /// Gets the text style to apply to tooltip content in the chart.
    /// </summary>
    public ChartTextStyle Tooltip { get; init; } = ChartTextStyle.Tooltip;
}
