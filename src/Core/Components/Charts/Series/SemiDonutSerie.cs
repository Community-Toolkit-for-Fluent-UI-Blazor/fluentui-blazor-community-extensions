using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering semi-donut charts using category items and semi-donut-specific options.
/// </summary>
public sealed class SemiDonutSerie
    : ChartSerie<RadialSlice>
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.SemiDonut;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);

    /// <summary>
    /// Gets or sets a value indicating whether the animation alternates direction on each pie.
    /// </summary>
    public bool AlternateAnimation { get; set; }
}
