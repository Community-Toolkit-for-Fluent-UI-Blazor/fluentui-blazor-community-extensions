namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the options for a hierarchy chart serie.
/// </summary>
public sealed class HierarchySerieOptions : IChartSerieOptions
{
    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the associated elements.
    /// </summary>
    public bool ShowLabels { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether values are displayed alongside the corresponding elements.
    /// </summary>
    public bool ShowValues { get; init; }

    /// <inheritdoc />
    public ChartAnimationOptions? Animation { get; set; }
}
