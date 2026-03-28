namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render a watermark, either as text or an image, onto a signature surface using a canvas
/// context.
/// </summary>
/// <remarks>The watermark can be customized with options such as opacity, font, color, alignment, padding,
/// rotation, and scaling with canvas DPI. This renderer supports both text and image watermarks, and applies the
/// specified settings to position and style the watermark appropriately. Use this class when you need to overlay a
/// watermark on a signature surface for visual branding or security purposes.</remarks>
internal sealed class WatermarkRenderer : ISurfaceRenderer<SurfaceWatermarkOptions>
{
    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        SurfaceWatermarkOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SurfaceWatermarkOptions options)
    {
        if (!options.Enabled)
        {
            return;
        }

        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new WatermarkLayer(payload));
    }
}
