namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a default implementation for building a surface render target from a stroke layer payload.
/// </summary>
/// <remarks>This builder adds layers to the target based on the properties present in the payload and the
/// specified export options. It conditionally includes view, background, grid, axes, content, and watermark layers as
/// appropriate. The builder is intended for use with stroke-based surface payloads and ensures that all included
/// elements are rendered in the correct order.</remarks>
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
            target.AddLayer(new BackgroundLayer(payload.Background));
        }

        if (payload.Grid is not null &&
            options.IncludeGrid)
        {
            target.AddLayer(new GridLayer(payload.Grid));
        }

        if (payload.Axes is not null &&
            options.IncludeAxes)
        {
            target.AddLayer(new AxesLayer(payload.Axes));
        }

        if (payload.Content is not null)
        {
            target.AddLayer(new StrokeLayer(payload.Content));
        }

        if (payload.Watermark is not null && options.IncludeWatermark)
        {
            target.AddLayer(new WatermarkLayer(payload.Watermark));
        }

        await target.FlushAsync();
    }
}
