using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a data series for rendering bar charts using category items and bar-specific options.
/// </summary>
public sealed class DonutSerie
    : ChartSerie<RadialSlice, RadialSerieOptions>
{
    /// <inheritdoc />
    public override ChartType ChartType => ChartType.Donut;

    /// <inheritdoc />
    protected internal override IEnumerable<double> Values => Items.Select(x => x.Value);

    /// <summary>
    /// Gets or sets a value indicating whether the animation alternates direction on each pie.
    /// </summary>
    public bool AlternateAnimation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current instance is part of a multi-donut.
    /// </summary>
    internal bool IsPartOfMultiDonut { get; set; }
}
