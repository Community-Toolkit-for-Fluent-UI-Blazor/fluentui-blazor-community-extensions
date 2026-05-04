namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a provider that generates chart themes based on specified requests.
/// </summary>
public interface IChartThemeProvider
{
    /// <summary>
    /// Creates a new chart theme based on the specified theme request.
    /// </summary>
    /// <param name="request">The theme request containing the parameters and settings used to generate the chart theme. Cannot be null.</param>
    /// <returns>A ChartTheme instance configured according to the provided request.</returns>
    Task<ChartTheme> CreateThemeAsync(ChartThemeRequest request);
}

