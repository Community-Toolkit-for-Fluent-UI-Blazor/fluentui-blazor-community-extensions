using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a chart series that displays multiple donut charts within a single chart area.
/// </summary>
/// <remarks>Use this class to visualize grouped or comparative data sets as multiple concentric or side-by-side
/// donut charts. Each donut chart is defined by an item in the collection, allowing for flexible representation of
/// complex data relationships.</remarks>
public sealed class MultiDonutSerie : ChartSerie
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.MultiDonut;

    /// <summary>
    /// Gets or sets the collection of donut series items to be rendered in the multi-donut chart.
    /// </summary>
    public List<DonutSerie> Series { get; init; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether to use alternate animation for the multi-donut series.
    /// </summary>
    public bool? AlternateAnimation { get; set; }

    /// <inheritdoc />
    protected internal override int ItemsCount => Series.Count;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Series.SelectMany(x => x.Values);

    /// <summary>
    /// Gets the raw collection of items to be displayed.
    /// </summary>
    internal override IReadOnlyList<ChartItem> RawItems => [];

    /// <summary>
    /// Gets or sets the palette mode for the multi-donut series, determining how colors are applied to the individual donut charts.
    /// </summary>
    internal DonutPaletteMode PaletteMode { get; set; }
}
