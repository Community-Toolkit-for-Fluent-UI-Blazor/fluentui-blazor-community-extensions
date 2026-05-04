namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Defines the contract for specifying options related to a chart series.
/// </summary>
/// <remarks>Implement this interface to provide configuration settings for individual chart series in custom
/// chart components.</remarks>
public interface IChartSerieOptions
{
    /// <summary>
    /// Gets the animation options for the chart series.
    /// </summary>
    ChartAnimationOptions? Animation { get; }
}
