namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents the interaction settings for a chart serie.
/// </summary>
public sealed class ChartSerieInteraction
{
    /// <summary>
    /// Gets a value indicating whether hover interactions are enabled for the chart.
    /// </summary>
    public bool HoverEnabled { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether press interactions are enabled for the chart.
    /// </summary>
    public bool PressEnabled { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether selection functionality is enabled.
    /// </summary>
    public bool SelectionEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether the chart serie participates in hit testing and can respond to pointer events.
    /// </summary>
    public bool HitTestVisible { get; init; } = true;
}
