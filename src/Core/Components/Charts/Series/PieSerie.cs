using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering bar charts using category items and bar-specific options.
/// </summary>
public sealed class PieSerie
    : ChartSerie<RadialSlice>
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.Pie;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);

    /// <summary>
    /// Gets or sets a value indicating whether the animation alternates direction on each pie.
    /// </summary>
    public bool AlternateAnimation { get; set; }
}
