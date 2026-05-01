namespace FluentUI.Blazor.Community.Components.Charts;

internal class HistogramModel
{
    public required double Min { get; init; }
    public required double Max { get; init; }
    public required double BinWidth { get; init; }
    public required int[] Counts { get; init; }
    public required double[] BinEdges { get; init; }
}
