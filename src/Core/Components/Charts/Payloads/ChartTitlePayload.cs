using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the chart title.
/// </summary>
public sealed record ChartTitlePayload : ILayerPayload
{
    /// <summary>
    /// Gets or sets the title of the chart.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the position of the chart title.
    /// </summary>
    public ChartRect Area { get; set; } = ChartRect.Empty;
}
