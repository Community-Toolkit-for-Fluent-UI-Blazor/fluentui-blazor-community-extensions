namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Defines interaction behavior for a chart item.
/// </summary>
public sealed class ChartItemInteraction
{
    /// <summary>
    /// Gets or sets a value indicating whether the component participates in hit testing and can respond to pointer events.
    /// </summary>
    public bool HitTestVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the padding applied to the hit test area of the chart.
    /// </summary>
    public double HitTestPadding { get; set; }

    /// <summary>
    /// Gets a value indicating if the chart item can respond to hover events.
    /// </summary>
    public bool Hoverable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the chart item can be pressed or clicked by the user.
    /// </summary>
    public bool Pressable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the chart item can be selected by user interaction.
    /// </summary>
    public bool Selectable { get; set; }
}
