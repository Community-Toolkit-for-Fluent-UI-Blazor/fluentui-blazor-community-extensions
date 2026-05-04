using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components.Surface.Engines;

/// <summary>
/// 
/// </summary>
public sealed class SurfaceAxisEngine
    : IAxisEngine<SurfaceViewOptions, SurfaceAxesOptions>
{
    /// <inheritdoc/>
    public AxisPayload? Build(SurfaceViewOptions view, SurfaceAxesOptions opt)
    {
        if (opt is null || !opt.Show)
        {
            return null;
        }

        var centerX = view.RenderLeft+ view.RenderWidth / 2.0;
        var centerY = view.RenderTop + view.RenderHeight / 2.0;

        return new AxisPayload
        {
            Color = opt.Color,
            Opacity = opt.Opacity,
            StrokeWidth = opt.StrokeWidth,
            DashArray = SurfaceMathUtils.ToDashArray(opt.DashArray),
            Layer = opt.Layer,

            XAxis = new AxisGeometryPayload
            {
                StartPoint = new(centerX - view.RenderWidth / 2, centerY),
                EndPoint = new(centerX + view.RenderWidth / 2, centerY),
                Ticks = [],
                Labels = []
            },

            YAxis = new AxisGeometryPayload
            {
                StartPoint = new(centerX, centerY - view.RenderHeight / 2),
                EndPoint = new(centerX, centerY + view.RenderHeight / 2),
                Ticks = [],
                Labels = []
            }
        };
    }
}
