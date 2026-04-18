using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using ClipPathLayer = FluentUI.Blazor.Community.Components.Components.Charts.Layers.ClipPathLayer;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using Xunit;

namespace Components.Tests.Components.Charts.Layers;

public class ChartLayersTests
{
    [Fact]
    public void ChartLayers_ExposeMetadata()
    {
        var bar = new BarLayer(new BarPayloadCollection(Guid.NewGuid().ToString(), []));
        var column = new ColumnLayer(new ColumnPayloadCollection([]));
        var line = new LineLayer(new LinePayload
        {
            ChartId = "chart",
            GroupId = "group",
            Id = "line",
            Index = 0,
            SerieIndex = 0,
            Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
            Path = new LinePathPayload { Id = "path", Smooth = false, Points = [] },
            Points = []
        });
        var pie = new PieLayer(new PiePayloadCollection { Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(), Radius = 1, TotalValue = 0, Slices = [] });
        var donut = new DonutLayer(new DonutPayloadCollection { Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(), Radius = 1, InnerRadius = 0, Slices = [] });
        var multi = new MultiDonutLayer(new MultiDonutPayloadCollection { Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(), Rings = [] });
        var clip = new ClipPathLayer(new ClipPathPayload { X = 0, Y = 0, Width = 1, Height = 1 });
        var layout = new ChartLayoutLayer(new ChartLayoutPayload(null, null, null));

        Assert.Equal("bar", bar.Key);
        Assert.Equal(LayerOrder.Content, bar.Order);
        Assert.Equal((int)ChartType.Bar, bar.Priority);

        Assert.Equal("column", column.Key);
        Assert.Equal(LayerOrder.Content, column.Order);
        Assert.Equal((int)ChartType.Column, column.Priority);

        Assert.Equal("line", line.Key);
        Assert.Equal(LayerOrder.Content, line.Order);
        Assert.Equal((int)ChartType.CategoryLine, line.Priority);

        Assert.Equal("pie", pie.Key);
        Assert.Equal(LayerOrder.Content, pie.Order);
        Assert.Equal((int)ChartType.Pie, pie.Priority);

        Assert.Equal("donut", donut.Key);
        Assert.Equal(LayerOrder.Content, donut.Order);
        Assert.Equal((int)ChartType.Donut, donut.Priority);

        Assert.Equal("multi-donut", multi.Key);
        Assert.Equal(LayerOrder.Content, multi.Order);
        Assert.Equal((int)ChartType.MultiDonut, multi.Priority);

        Assert.Equal("clip-path", clip.Key);
        Assert.Equal(LayerOrder.Background, clip.Order);
        Assert.Equal(0, clip.Priority);

        Assert.Equal("chartlayout", layout.Key);
        Assert.Equal(LayerOrder.Front, layout.Order);
    }

    [Fact]
    public void SurfaceLayers_RespectOrders()
    {
        var axesPayload = new AxisPayload { Layer = AxesLayerOrder.Background };
        var axes = new AxesLayer(axesPayload);

        var gridPayload = new GridPayload { Layer = GridLayerOrder.Background, HorizontalLines = [], VerticalLines = [], Points = [] };
        var grid = new GridLayer(gridPayload);

        Assert.Equal(LayerOrder.Background, axes.Order);
        Assert.Equal(LayerOrder.Background, grid.Order);
    }
}
