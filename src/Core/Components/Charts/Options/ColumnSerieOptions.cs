namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents configuration options for a column series in a chart.
/// </summary>
public sealed class ColumnSerieOptions : CategorySerieOptions
{
    /// <summary>
    /// Gets the width of each column in relative units (0–1).
    /// </summary>
    public double ColumnWidth { get; init; } = 0.8;
}

