namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the typography settings for a chart, including styles for various text elements such as title, subtitle,
///  axis, legend, labels, and values.
/// </summary>
public sealed record ChartTypographyOverride
{
    /// <summary>
    /// Gets the text style applied to the chart title.
    /// </summary>
    public ChartTextStyle? Title { get; init; }

    /// <summary>
    /// Gets the text style applied to the chart subtitle.
    /// </summary>
    public ChartTextStyle? Subtitle { get; init; }

    /// <summary>
    /// Gets the text style applied to axis labels in the chart.
    /// </summary>
    public ChartTextStyle? Axis { get; init; }

    /// <summary>
    /// Gets the text style applied to the chart legend.
    /// </summary>
    public ChartTextStyle? Legend { get; init; }

    /// <summary>
    /// Gets the text style applied to the chart label.
    /// </summary>
    public ChartTextStyle? Label { get; init; }

    /// <summary>
    /// Gets the text style applied to the chart values.
    /// </summary>
    public ChartTextStyle? Value { get; init; }

    /// <summary>
    /// Gets the text style used for small labels in the chart.
    /// </summary>
    public ChartTextStyle? SmallLabel { get; init; }

    /// <summary>
    /// Gets the text style to apply to tooltip content in the chart.
    /// </summary>
    public ChartTextStyle? Tooltip { get; init; }
}
