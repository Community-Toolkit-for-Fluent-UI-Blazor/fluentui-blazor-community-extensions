namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// 
/// </summary>
internal sealed class DefaultSurfaceTargetBuilder
    : ISurfaceTargetBuilder<StrokeLayerPayload>
{
    /// <inheritdoc />
    public async ValueTask BuildAsync(
        ISurfaceRenderTarget target,
        SurfacePayload<StrokeLayerPayload> payload,
        SurfaceExportOptions options)
    {
        if (payload.View is not null && options.IncludeView)
        {
            target.SetView(payload.View);
        }

        if (payload.Background is not null && options.IncludeBackground)
        {
            target.StaticBackLayer.SetBackground(payload.Background);
        }

        if (payload.Grid is not null && options.IncludeGrid && payload.Grid.Layer == GridLayer.Background)
        {
            target.StaticBackLayer.SetGrid(payload.Grid);
        }

        if (payload.Axes is not null && options.IncludeAxes && payload.Axes.Layer == GridLayer.Background)
        {
            target.StaticFrontLayer.SetAxes(payload.Axes);
        }

        if (payload.Content is not null)
        {
            target.DynamicLayer.SetStrokeLayer(payload.Content);
        }

        if (payload.Grid is not null && options.IncludeGrid && payload.Grid.Layer == GridLayer.Foreground)
        {
            target.StaticBackLayer.SetGrid(payload.Grid);
        }

        if (payload.Axes is not null && options.IncludeAxes && payload.Axes.Layer == GridLayer.Foreground)
        {
            target.StaticFrontLayer.SetAxes(payload.Axes);
        }

        if (payload.Watermark is not null && options.IncludeWatermark)
        {
            target.WaterMark.SetWatermark(payload.Watermark);
        }

        await target.FlushAsync();
    }
}
