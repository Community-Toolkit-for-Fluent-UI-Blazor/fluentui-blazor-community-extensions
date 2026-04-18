namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the options for a category chart serie.
/// </summary>
public abstract class CategorySerieOptions : IChartSerieOptions
{
    /// <summary>
    /// Gets a value indicating whether categories are sorted.
    /// </summary>
    public bool Sort { get; set; }

    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the associated elements.
    /// </summary>
    public bool ShowLabels { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether values are displayed alongside the corresponding elements.
    /// </summary>
    public bool ShowValues { get; set; }

    /// <inheritdoc />
    public ChartAnimationOptions? Animation { get; set; }
}
