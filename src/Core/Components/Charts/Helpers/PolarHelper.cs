namespace FluentUI.Blazor.Community.Components.Charts.Helpers;

internal static class PolarHelper
{
    public static (double cx, double cy, double maxRadius) GetPolarFrame(ChartContext ctx)
    {
        var plot = ctx.PlotArea;
        var cx = plot.X + plot.Width / 2;
        var cy = plot.Y + plot.Height / 2;
        var maxRadius = Math.Min(plot.Width, plot.Height) / 2;

        return (cx, cy, maxRadius);
    }
}
