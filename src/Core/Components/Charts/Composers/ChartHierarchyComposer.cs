using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;
using CO = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;

namespace FluentUI.Blazor.Community.Components.Charts.Composers;

internal sealed class ChartHierarchyComposer : ISurfaceComposer<CO>
{
    private readonly string _chartId;
    private readonly Func<ChartContext> _context;
    private readonly Func<IEnumerable<HierarchySerie>> _series;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartHierarchyComposer"/> class with the specified chart context, series collection, and chart identifier.
    /// </summary>
    /// <param name="context">The chart context for composition.</param>
    /// <param name="series">The collection of hierarchy series to be rendered.</param>
    /// <param name="chartId">The unique identifier of the chart, used for interaction payloads.</param>
    public ChartHierarchyComposer(
        string chartId,
        Func<ChartContext> context,
        Func<IEnumerable<HierarchySerie>> series)
    {
        _chartId = chartId;
        _context = context;
        _series = series;
    }

    /// <inheritdoc/>
    public bool Compose(ISurfaceRenderTarget target, CO options)
    {
        var filtered = _series().Where(s => s.IsVisible).ToList();

        if (filtered.Count == 0)
        {
            return false;
        }

        var ctx = _context();
        var treemap = new List<HierarchySerie>();
        var tree = new List<HierarchySerie>();
        var radialTree = new List<HierarchySerie>();
        var sunburst = new List<HierarchySerie>();
        var icicle = new List<HierarchySerie>();
        var dendrogram = new List<HierarchySerie>();
        var partition = new List<HierarchySerie>();

        foreach (var serie in filtered)
        {
            switch (serie.HierarchyChartType)
            {
                case HierarchyChartType.Treemap:
                    treemap.Add(serie);
                    break;

                case HierarchyChartType.Tree:
                    tree.Add(serie);
                    break;

                case HierarchyChartType.RadialTree:
                    radialTree.Add(serie);
                    break;

                case HierarchyChartType.Sunburst:
                    sunburst.Add(serie);
                    break;

                case HierarchyChartType.Icicle:
                    icicle.Add(serie);
                    break;

                case HierarchyChartType.Dendrogram:
                    dendrogram.Add(serie);
                    break;

                case HierarchyChartType.Partition:
                    partition.Add(serie);
                    break;
            }
        }

        if (sunburst.Count > 0)
        {
            BuildSunburstGroup(target, sunburst, ctx, options);
        }

        if (icicle.Count > 0)
        {
            BuildIcicleGroup(target, icicle, ctx, options);
        }

        if (treemap.Count > 0)
        {
            BuildTreemapGroup(target, treemap, ctx, options);
        }

        if (tree.Count > 0)
        {
            BuildTreeGroup(target, tree, ctx, options);
        }

        if (radialTree.Count > 0)
        {
            BuildRadialTreeGroup(target, radialTree, ctx, options);
        }

        if (dendrogram.Count > 0)
        {
            BuildDendrogramGroup(target, dendrogram, ctx, options);
        }

        if (partition.Count > 0)
        {
            BuildPartitionGroup(target, partition, ctx, options);
        }

        return true;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ComposeAsync(ISurfaceRenderTarget target, CO options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }

    private static void BuildPartitionGroup(
        ISurfaceRenderTarget target,
        List<HierarchySerie> partition,
        ChartContext ctx,
        CO options)
    {
        var serie = partition[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        HierarchyValueAggregator.Aggregate(model);
        PartitionLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.PartitionStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: false));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Partition
        }));
    }

    private static void BuildDendrogramGroup(ISurfaceRenderTarget target, List<HierarchySerie> dendrogram, ChartContext ctx, CO options)
    {
        var serie = dendrogram[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        HierarchyValueAggregator.Aggregate(model);
        DendrogramLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.DendrogramStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: false));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Dendrogram
        }));
    }

    private static void BuildRadialTreeGroup(ISurfaceRenderTarget target, List<HierarchySerie> radialTree, ChartContext ctx, CO options)
    {
        var serie = radialTree[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        RadialTreeLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.RadialTreeStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: true));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.RadialTree
        }));
    }

    private static void BuildTreeGroup(ISurfaceRenderTarget target, List<HierarchySerie> tree, ChartContext ctx, CO options)
    {
        var serie = tree[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        TreeLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.TreeStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: false));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Tree
        }));
    }

    private static void BuildTreemapGroup(
        ISurfaceRenderTarget target,
        List<HierarchySerie> treemap,
        ChartContext ctx,
        CO options)
    {
        var serie = treemap[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        HierarchyValueAggregator.Aggregate(model);
        var layoutNodes = TreemapLayoutEngine.Layout(model, ctx.PlotArea);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.TreemapStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: false));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Treemap
        }));
    }

    private static void BuildIcicleGroup(ISurfaceRenderTarget target, List<HierarchySerie> icicle, ChartContext ctx, CO options)
    {
        var serie = icicle[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        IcicleLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.IcicleStyle;

        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: false));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Icicle
        }));
    }

    private static void BuildSunburstGroup(ISurfaceRenderTarget target, List<HierarchySerie> sunburst, ChartContext ctx, CO options)
    {
        var serie = sunburst[0];
        var root = HierarchyBuilder.Build(serie.Items, serie.Group);
        var model = HierarchyModelBuilder.Build(root);
        SunburstLayoutEngine.Layout(model, ctx.PlotArea);
        var layoutNodes = HierarchyLayoutFlattener.Flatten(model);
        var payloads = new List<HierarchyNodePayload>(layoutNodes.Count);
        var defaultStyle = options.SunburstStyle;
        var leafColorIndex = 0;

        for (var i = 0; i < layoutNodes.Count; i++)
        {
            var node = layoutNodes[i];
            var isLeaf = node.Source.Children.Count == 0;
            node.LeafColorIndex = isLeaf ? leafColorIndex++ : -1;

            payloads.Add(CreateHierarchyPayload(
                i,
                node,
                serie,
                ctx,
                defaultStyle,
                isRadial: true));
        }

        target.AddLayer(new HierarchyLayer(new HierarchyPayload
        {
            Id = serie.Id,
            Nodes = payloads,
            Type = ChartType.Sunburst
        }));
    }

    private static HierarchyNodePayload CreateHierarchyPayload(
        int index,
        HierarchyLayoutNode node,
        HierarchySerie serie,
        ChartContext ctx,
        ChartHierarchyStyle defaultStyle,
        bool isRadial)
    {
        var item = node.Source.SourceItem;
        var isLeaf = node.Source.Children.Count == 0;
        var serieIndex = isLeaf ? node.LeafColorIndex : -1;

        return new HierarchyNodePayload
        {
            Id = item?.Id,
            GroupId = serie.Id,
            ChartId = ctx.ChartId,
            Index = index,
            SerieIndex = serieIndex,
            IsLeaf = isLeaf,
            Normal = ChartStyleResolver.Resolve(item?.Style?.Normal, defaultStyle.Normal),
            Hover = ChartStyleResolver.Resolve(item?.Style?.Hover, defaultStyle.Hover),
            Pressed = ChartStyleResolver.Resolve(item?.Style?.Pressed, defaultStyle.Pressed),
            Selected = ChartStyleResolver.Resolve(item?.Style?.Selected, defaultStyle.Selected),
            Disabled = ChartStyleResolver.Resolve(item?.Style?.Disabled, defaultStyle.Disabled),
            Animation = item?.Animation,
            AnimationEnabled = serie.AnimationEnabled,
            InteractionState = item?.InteractionState ?? ChartInteractionState.Normal,
            IsDisabled = !item?.IsVisible ?? true,
            Tooltip = new ChartTooltipPayload
            {
            },
            X = node.Rect.X,
            Y = node.Rect.Y,
            Width = node.Rect.Width,
            Height = node.Rect.Height,
            Value = node.Source.Value,
            Label = item?.Label ?? item?.Name ?? node.Source.Name,
            Depth = node.Source.Depth,
            ParentIndex = node.Source.ParentIndex,
            ChildrenRange = node.Source.ChildrenRange,
            StartAngle = isRadial ? node.StartAngle : 0d,
            EndAngle = isRadial ? node.EndAngle : 0d,
            InnerRadius = isRadial ? node.InnerRadius : 0d,
            OuterRadius = isRadial ? node.OuterRadius : 0d
        };
    }
}
