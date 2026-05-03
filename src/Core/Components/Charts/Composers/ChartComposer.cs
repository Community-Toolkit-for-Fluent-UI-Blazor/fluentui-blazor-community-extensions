using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components;

internal sealed class ChartComposer
{
    private readonly List<ISurfaceComposer<CO>> _commonComposers = [];
    private readonly List<ISurfaceComposer<CO>> _categoryComposers = [];
    private readonly List<ISurfaceComposer<CO>> _polarSeriesComposers = [];
    private readonly List<ISurfaceComposer<CO>> _xySeriesComposers = [];
    private readonly List<ISurfaceComposer<CO>> _histogramComposers = [];
    private readonly List<ISurfaceComposer<CO>> _circularComposers = [];
    private readonly List<ISurfaceComposer<CO>> _hierarchyComposers = [];

    public ChartComposer AddCommonComposers(params ISurfaceComposer<CO>[] composers)
    {
        _commonComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddHierarchyComposers(params ISurfaceComposer<CO>[] composers)
    {
        _hierarchyComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddCircularComposers(params ISurfaceComposer<CO>[] composers)
    {
        _circularComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddCategoryComposers(params ISurfaceComposer<CO>[] composers)
    {
        _categoryComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddPolarComposers(params ISurfaceComposer<CO>[] composers)
    {
        _polarSeriesComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddXYComposers(params ISurfaceComposer<CO>[] composers)
    {
        _xySeriesComposers.AddRange(composers);

        return this;
    }

    public ChartComposer AddHistogramComposers(params ISurfaceComposer<CO>[] composers)
    {
        _histogramComposers.AddRange(composers);

        return this;
    }

    public bool Compose(
        IEnumerable<ChartSerie> series,
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;
        var hasCategory = false;
        var hasXY = false;
        var hasPolar = false;
        var hasHistogram = false;
        var hasCircular = false;
        var hasHierarchy = false;

        foreach (var serie in series)
        {
            var t = serie.ChartType;
            var cat = ChartTypeInfo.Categories[t];

            switch (cat)
            {
                case ChartFamily.Category:
                    hasCategory = true;
                    break;

                case ChartFamily.XY:
                    hasXY = true;
                    break;

                case ChartFamily.Polar:
                    hasPolar = true;
                    break;

                case ChartFamily.Histogram:
                    hasHistogram = true;
                    break;

                case ChartFamily.Circular:
                    hasCircular = true;
                    break;

                case ChartFamily.Hierarchy:
                    hasHierarchy = true;
                    break;
            }
        }

        foreach (var composer in _commonComposers)
        {
            composed |= composer.Compose(renderTarget, options);
        }

        if (hasCategory)
        {
            composed |= Compose(_categoryComposers, renderTarget, options);
        }

        if (hasXY)
        {
            composed |= Compose(_xySeriesComposers, renderTarget, options);
        }

        if (hasPolar)
        {
            composed |= Compose(_polarSeriesComposers, renderTarget, options);
        }

        if (hasHistogram)
        {
            composed |= Compose(_histogramComposers, renderTarget, options);
        }

        if (hasCircular)
        {
            composed |= Compose(_circularComposers, renderTarget, options);
        }

        if (hasHierarchy)
        {
            composed |= Compose(_hierarchyComposers, renderTarget, options);
        }

        return composed;
    }

    /// <inheritdoc/>
    public async ValueTask<bool> ComposeAsync(
        IEnumerable<ChartSerie> series,
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;
        var hasCategory = false;
        var hasXY = false;
        var hasPolar = false;
        var hasHistogram = false;
        var hasCircular = false;

        foreach (var serie in series)
        {
            var t = serie.ChartType;
            var cat = ChartTypeInfo.Categories[t];

            switch (cat)
            {
                case ChartFamily.Category:
                    hasCategory = true;
                    break;

                case ChartFamily.XY:
                    hasXY = true;
                    break;

                case ChartFamily.Polar:
                    hasPolar = true;
                    break;

                case ChartFamily.Histogram:
                    hasHistogram = true;
                    break;

                case ChartFamily.Circular:
                    hasCircular = true;
                    break;
            }
        }

        foreach (var composer in _commonComposers)
        {
            composed |= await composer.ComposeAsync(renderTarget, options);
        }

        if (hasCategory)
        {
            composed |= await ComposeAsync(_categoryComposers, renderTarget, options);
        }

        if (hasXY)
        {
            composed |= await ComposeAsync(_xySeriesComposers, renderTarget, options);
        }

        if (hasPolar)
        {
            composed |= await ComposeAsync(_polarSeriesComposers, renderTarget, options);
        }

        if (hasHistogram)
        {
            composed |= await ComposeAsync(_histogramComposers, renderTarget, options);
        }

        if (hasCircular)
        {
            composed |= await ComposeAsync(_circularComposers, renderTarget, options);
        }

        return composed;
    }

    /// <summary>
    /// Compose the given composers with the provided render target and options.
    /// </summary>
    /// <param name="composers">The list of composers to compose.</param>
    /// <param name="renderTarget">The render target to use for composition.</param>
    /// <param name="options">The options to use for composition.</param>
    /// <returns>Returns <see langword="true" /> if any composer successfully composed; otherwise, <see langword="false"/>.</returns>
    private static bool Compose(
        List<ISurfaceComposer<CO>> composers,
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;

        foreach (var composer in composers)
        {
            composed |= composer.Compose(renderTarget, options);
        }

        return composed;
    }

    /// <summary>
    /// Compose the given composers with the provided render target and options.
    /// </summary>
    /// <param name="composers">The list of composers to compose.</param>
    /// <param name="renderTarget">The render target to use for composition.</param>
    /// <param name="options">The options to use for composition.</param>
    /// <returns>Returns <see langword="true" /> if any composer successfully composed; otherwise, <see langword="false"/>.</returns>
    private static async ValueTask<bool> ComposeAsync(
        List<ISurfaceComposer<CO>> composers,
        ISurfaceRenderTarget renderTarget,
        CO options)
    {
        var composed = false;

        foreach (var composer in composers)
        {
            composed |= await composer.ComposeAsync(renderTarget, options);
        }

        return composed;
    }
}
