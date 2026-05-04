namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

internal sealed record XYPoint
{
    public required double X { get; init; }
    public required double Y { get; init; }

    public required double RawX { get; init; }
    public required double RawY { get; init; }

    public required double Radius { get; init; }

    public required double Value { get; init; }

    public required int Index { get; init; }
}
