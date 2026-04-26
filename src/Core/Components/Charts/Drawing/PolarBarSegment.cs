namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

internal sealed record PolarBarSegment
{
    public required int Index { get; init; }
    public required double CenterX { get; init; }
    public required double CenterY { get; init; }
    public required double StartAngle { get; init; }
    public required double EndAngle { get; init; }
    public required double InnerRadius { get; init; }
    public required double OuterRadius { get; init; }
    public required double Value { get; init; }
    public required string Category { get; init; }
}
