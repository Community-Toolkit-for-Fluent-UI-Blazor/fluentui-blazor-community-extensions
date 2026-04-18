namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a provider for generating chart palettes based on specific requests.
/// </summary>
public interface IChartPaletteProvider
{
    /// <summary>
    /// Generates a chart palette based on the specified palette request.
    /// </summary>
    /// <param name="request">The palette request that defines the parameters and options for generating the chart palette. Cannot be null.</param>
    /// <returns>A ChartPalette instance configured according to the provided request.</returns>
    Task<ChartPalette> GenerateAsync(ChartPaletteRequest request);
}
