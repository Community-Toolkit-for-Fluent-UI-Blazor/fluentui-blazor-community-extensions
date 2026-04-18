using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the chart subtitle.
/// </summary>
public sealed record ChartSubtitlePayload : ILayerPayload
{
    /// <summary>
    /// Gets or sets the subtitle of the chart.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the position of the chart subtitle.
    /// </summary>
    public ChartRect Area { get; set; } = ChartRect.Empty;
}
