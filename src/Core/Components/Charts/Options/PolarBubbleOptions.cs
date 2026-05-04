namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the options for a polar bubble chart serie.
/// </summary>
public sealed class PolarBubbleOptions : PolarSerieOptions
{
    /// <summary>
    /// Gets the minimum radius of the bubbles.
    /// </summary>
    public double MinRadius { get; init; } = 3;

    /// <summary>
    /// Gets the maximum radius of the bubbles.
    /// </summary>
    public double MaxRadius { get; init; } = 30;
}

