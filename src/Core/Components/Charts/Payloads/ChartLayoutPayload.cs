namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for the chart layout, containing information about the title, subtitle, and legend of the chart.
/// </summary>
/// <param name="TitlePayload">The payload containing information about the chart title.</param>
/// <param name="SubtitlePayload">The payload containing information about the chart subtitle.</param>
/// <param name="LegendPayload">The payload containing information about the chart legend.</param>
public sealed record ChartLayoutPayload(
    ChartTitlePayload? TitlePayload,
    ChartSubtitlePayload? SubtitlePayload,
    ChartLegendPayload? LegendPayload) : ILayerPayload
{
}
