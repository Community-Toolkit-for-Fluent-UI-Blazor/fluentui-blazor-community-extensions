namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a bubble point with X, Y and Radius.
/// </summary>
public sealed class BubblePoint : XYPoint
{
    /// <summary>
    /// Gets the radius value for the bubble.
    /// </summary>
    public required double Radius { get; init; }
}

