namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents a point in a bubble chart, including its position, size, and associated data values.
/// </summary>
internal sealed class BubblePoint
{
    public required string Id { get; init; }
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Radius { get; init; }
    public required double Value { get; init; }
}
