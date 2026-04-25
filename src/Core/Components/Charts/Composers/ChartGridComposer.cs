using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

/// <summary>
/// Represents a chart axes composer that generates the necessary geometry and payload for rendering chart axes.
/// </summary>
/// <param name="context">The chart context containing relevant information for axis composition</param>
/// <param name="gridOptions">The options defining the configuration and appearance of the chart grid</param>
internal sealed class ChartGridComposer(
    Func<ChartContext> context,
    Func<ChartGridOptions> gridOptions)
    : ISurfaceComposer<CO>
{
    /// <summary>
    /// Provides the engine responsible for managing chart grid calculations and rendering.
    /// </summary>
    private readonly ChartGridEngine _gridEngine = new();

    /// <inheritdoc/>
    public bool Compose(
        ISurfaceRenderTarget target,
        CO options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        var mergedGridOptions = Merge(gridOptions(), options.DefaultGridOptions);

        if (!mergedGridOptions.ShowHorizontal &&
            !mergedGridOptions.ShowVertical)
        {
            return false;
        }

        var payload = _gridEngine.Build(context(), mergedGridOptions);

        if (payload is null)
        {
            return false;
        }

        target.AddLayer(new GridLayer(payload));

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }

    /// <summary>
    /// Merges local grid options with global grid options.
    /// </summary>
    /// <param name="local">Local grid options that may override global settings</param>
    /// <param name="global">Global grid options that provide default settings for the chart grid</param>
    /// <returns>Returns a new instance of <see cref="ChartGridOptions"/> that combines local and global settings, with local options taking precedence over global options where specified.</returns>
    private static ChartGridOptions Merge(
        ChartGridOptions local,
        ChartGridOptions global)
    {
        return new ChartGridOptions
        {
            // Behavior : local options take precedence over global options.
            ShowHorizontal = local.ShowHorizontal,
            ShowVertical = local.ShowVertical,
            SnapToTicks = local.SnapToTicks,

            // Style : local options take precedence over global options if specified, otherwise global options are used.
            DisplayMode = local.DisplayMode,
            CellSize = local.CellSize,
            Color = local.Color ?? global.Color,
            Opacity = local.Opacity,
            BoldEvery = local.BoldEvery,
            StrokeWidth = local.StrokeWidth,
            DashArray = local.DashArray ?? global.DashArray,
            PointRadius = local.PointRadius,
            Layer = local.Layer
        };
    }
}
