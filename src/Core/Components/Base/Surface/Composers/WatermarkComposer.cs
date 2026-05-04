namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render a watermark, either as text or an image, onto a signature surface using a canvas
/// context.
/// </summary>
/// <remarks>The watermark can be customized with options such as opacity, font, color, alignment, padding,
/// rotation, and scaling with canvas DPI. This renderer supports both text and image watermarks, and applies the
/// specified settings to position and style the watermark appropriately. Use this class when you need to overlay a
/// watermark on a signature surface for visual branding or security purposes.</remarks>
internal sealed class WatermarkComposer : ISurfaceComposer<SurfaceWatermarkOptions>
{
    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        SurfaceWatermarkOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }

    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        SurfaceWatermarkOptions options)
    {
        if (!options.Enabled)
        {
            return false;
        }

        var payload = PayloadFactory.Create(options);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new WatermarkLayer(payload));

        return true;
    }
}
