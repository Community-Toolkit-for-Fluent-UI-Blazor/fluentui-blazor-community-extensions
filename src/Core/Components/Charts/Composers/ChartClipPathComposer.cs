using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Components.Charts.Layers;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Provides functionality to compose SVG clip paths for chart rendering surfaces using the specified chart context.
/// </summary>
/// <remarks>This class is intended for internal use in chart rendering scenarios where SVG clip paths are
/// required to constrain drawing operations. It implements the ISurfaceComposer interface for integration with the
/// rendering pipeline.</remarks>
/// <param name="context">A delegate that returns the current chart context used to configure the clip path composition.</param>
internal sealed class ChartClipPathComposer(Func<ChartContext> context)
    : ISurfaceComposer<CO>
{
    public void Compose(ISurfaceRenderTarget target, CO options)
    {
        var ctx = context();

        var payload = new ClipPathPayload()
        {
            X = ctx.PlotArea.X,
            Y = ctx.PlotArea.Y,
            Width = ctx.PlotArea.Width,
            Height = ctx.PlotArea.Height,
        };

        ctx.ClipPathId = payload.Id;

        target.AddLayer(new ClipPathLayer(payload));
    }

    /// <inheritdoc />
    public ValueTask ComposeAsync(ISurfaceRenderTarget target, CO options)
    {
        Compose(target, options);

        return ValueTask.CompletedTask;
    }
}
