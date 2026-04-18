using FluentUI.Blazor.Community.Components.Charts.Engines;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using CAO = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a chart axes composer that generates the necessary geometry and payload for rendering chart axes.
/// </summary>
/// <param name="context">The chart context containing relevant information for axis composition</param>
/// <param name="axisOptions">The options defining the configuration and appearance of the chart axes</param>
internal sealed class ChartAxesComposer(
    Func<ChartContext> context,
    Func<CAO> axisOptions)
    : ISurfaceComposer<CO>
{
    /// <summary>
    /// Provides the engine responsible for managing chart axis calculations and rendering.
    /// </summary>
    private readonly ChartAxisEngine _axisEngine = new();

    /// <inheritdoc/>
    public void Compose(
        ISurfaceRenderTarget target,
        CO options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        var mergedAxisOptions = Merge(axisOptions(), options.DefaultAxisOptions);

        if (!mergedAxisOptions.Show)
        {
            return;
        }

        var payload = _axisEngine.Build(context(), mergedAxisOptions);

        if (payload is null)
        {
            return;
        }

        target.AddLayer(new AxesLayer(payload));
    }

    /// <inheritdoc/>
    public ValueTask ComposeAsync(
        ISurfaceRenderTarget target,
        CO options)
    {
        Compose(target, options);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Merges local axis options with global axis options.
    /// </summary>
    /// <param name="local">Local axis options that may override global settings</param>
    /// <param name="global">Global axis options that provide default settings for the chart axes</param>
    /// <returns>Returns a new instance of <see cref="ChartAxisOptions"/> that combines the local and global options, giving precedence to local settings where specified.</returns>
    private static CAO Merge(
        CAO local,
        CAO global)
    {
        return new CAO
        {
            // Behavior : local overrides global
            Show = local.Show,
            ShowTicks = local.ShowTicks,
            ShowLabels = local.ShowLabels,
            TickLength = local.TickLength,
            MaxTicks = local.MaxTicks,
            LabelOffset = local.LabelOffset,
            NumericFormat = local.NumericFormat,

            // Style : local overrides global
            Color = local.Color ?? global.Color,
            Opacity = local.Opacity,
            StrokeWidth = local.StrokeWidth,
            DashArray = local.DashArray ?? global.DashArray,
            Layer = local.Layer,
            LabelOptions = local.LabelOptions ?? global.LabelOptions ?? new(),
        };
    }
}
