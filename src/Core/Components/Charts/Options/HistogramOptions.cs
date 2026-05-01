namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the configuration options for a histogram series in a chart.
/// </summary>
public sealed class HistogramOptions : XYSeriesOptions
{
    /// <summary>
    /// Gets or sets the number of bin.
    /// </summary>
    public int BinCount { get; set; } = 10;
}
