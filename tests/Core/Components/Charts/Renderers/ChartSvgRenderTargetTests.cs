using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Layers;
using ClipPathLayer = FluentUI.Blazor.Community.Components.Components.Charts.Layers.ClipPathLayer;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Renderers;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Enums;
using ChartOptions = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.Charts.Renderers;

public class ChartSvgRenderTargetTests
{
    [Fact]
    public void AddLayer_UnsupportedLayer_Throws()
    {
        var target = CreateTarget();

        Assert.Throws<InvalidOperationException>(() => target.AddLayer(new FakeLayer()));
    }

    [Fact]
    public async Task FlushAsync_RendersAllLayers()
    {
        var target = CreateTarget();

        target.AddLayer(BuildAxesLayer());
        target.AddLayer(BuildGridLayer());
        target.AddLayer(new LineLayer(BuildLinePayload()));
        target.AddLayer(new BarLayer(BuildBarPayloads()));
        target.AddLayer(new ColumnLayer(BuildColumnPayloads()));
        target.AddLayer(new PieLayer(BuildPiePayloads()));
        target.AddLayer(new DonutLayer(BuildDonutPayloads()));
        target.AddLayer(new MultiDonutLayer(BuildMultiDonutPayloads()));
        target.AddLayer(BuildClipPathLayer());
        target.AddLayer(new ChartLayoutLayer(BuildLayoutPayload()));

        await target.FlushAsync();

        var svg = new MarkupString(target.GetNativeHandle()?.ToString() ?? string.Empty);

        Assert.Contains("chart-axis", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-grid", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-bars", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-columns", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("line-", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-pie", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-donut", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart-layout", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("clipPath", svg.Value, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FlushAsync_UsesViewPayloadDimensions()
    {
        var target = CreateTarget();

        target.SetView(new ViewPayload { RenderWidth = 333, RenderHeight = 222 });
        target.AddLayer(new LineLayer(BuildLinePayload()));

        await target.FlushAsync();

        var svg = new MarkupString(target.GetNativeHandle()?.ToString() ?? string.Empty);

        Assert.Contains("width=\"333\"", svg.Value, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("height=\"222\"", svg.Value, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task FlushAsync_ThrowsWhenThemeContextMissing()
    {
        var context = BuildChartContext();
        var target = new ChartSvgRenderTarget(
            "chart",
            () => context,
            () => null!,
            () => new ChartOptions());

        target.AddLayer(new LineLayer(BuildLinePayload()));

        await Assert.ThrowsAsync<InvalidOperationException>(() => target.FlushAsync().AsTask());
    }

    private static ChartSvgRenderTarget CreateTarget()
    {
        var context = BuildChartContext();
        var theme = BuildThemeContext();

        return new ChartSvgRenderTarget(
            "chart",
            () => context,
            () => theme,
            () => new ChartOptions());
    }

    private static ChartContext BuildChartContext()
    {
        return new ChartContext
        {
            ChartArea = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartRect(0, 0, 200, 120),
            PlotArea = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartRect(10, 10, 180, 100)
        };
    }

    private static ChartThemeContext BuildThemeContext()
    {
        return new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(5, 5, 5)]
                },
                Typography = new ChartTypography
                {
                    Label = new ChartTextStyle { FontFamily = "Segoe", FontSize = 12, Color = new Srgb8(1, 1, 1) },
                    Legend = new ChartTextStyle { FontFamily = "Segoe", FontSize = 12, Color = new Srgb8(2, 2, 2) },
                    Title = new ChartTextStyle { FontFamily = "Segoe", FontSize = 14, Color = new Srgb8(3, 3, 3) },
                    Subtitle = new ChartTextStyle { FontFamily = "Segoe", FontSize = 10, Color = new Srgb8(4, 4, 4) }
                },
                Layout = new ChartLayout()
            },
            ComputedValues = new ChartComputedValues
            {
                FinalAxisThickness = 2,
                FinalGridColor = new Srgb8(20, 20, 20),
                FinalGridThickness = 1,
                FinalStrokeThickness = 2,
                FinalBarRadius = 1
            }
        };
    }

    private static AxisPayload BuildAxisPayload()
    {
        return new AxisPayload
        {
            XAxis = new AxisGeometryPayload
            {
                StartPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 100),
                EndPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(200, 100),
                Ticks = [new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(100, 100)],
                Labels =
                [
                    new AxisLabelPayload
                    {
                        Text = "X",
                        Position = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(100, 110),
                        Anchor = SvgTextAnchor.Middle
                    }
                ]
            },
            YAxis = new AxisGeometryPayload
            {
                StartPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 0),
                EndPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 100),
                Ticks = [new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 50)],
                Labels =
                [
                    new AxisLabelPayload
                    {
                        Text = "Y",
                        Position = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 50),
                        Anchor = SvgTextAnchor.Middle
                    }
                ]
            }
        };
    }

    private static GridPayload BuildGridPayload()
    {
        return new GridPayload
        {
            DisplayMode = GridDisplayMode.Lines,
            Opacity = 0.5,
            HorizontalLines =
            [
                new GridLinePayload
                {
                    StartPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 50),
                    EndPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(200, 50)
                }
            ],
            VerticalLines =
            [
                new GridLinePayload
                {
                    StartPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(100, 0),
                    EndPoint = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(100, 100)
                }
            ],
            Points = []
        };
    }

    private static LinePayload BuildLinePayload()
    {
        return new LinePayload
        {
            ChartId = "chart",
            GroupId = "group",
            Id = "line",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            Path = new LinePathPayload
            {
                Id = "path",
                Smooth = false,
                Points =
                [
                    new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(0, 0),
                    new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(10, 10)
                ]
            },
            Points =
            [
                new LinePointPayload
                {
                    ChartId = "chart",
                    GroupId = "group",
                    Id = "pt-1",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new ChartVisualStateStyle(),
                    X = 0,
                    Y = 0,
                    Value = 1,
                    CategoryIndex = 0
                }
            ]
        };
    }

    private static BarPayloadCollection BuildBarPayloads()
    {
        return new BarPayloadCollection(Guid.NewGuid().ToString(),
        [
            new BarPayload
            {
                ChartId = "chart",
                GroupId = "group",
                Id = "bar",
                Index = 0,
                SerieIndex = 0,
                Normal = new ChartVisualStateStyle(),
                X = 10,
                Y = 10,
                Width = 20,
                Height = 30,
                CategoryIndex = 0,
                Value = 5
            }
        ]);
    }

    private static ColumnPayloadCollection BuildColumnPayloads()
    {
        return new ColumnPayloadCollection(
        [
            new ColumnPayload
            {
                ChartId = "chart",
                GroupId = "group",
                Id = "col",
                Index = 0,
                SerieIndex = 0,
                Normal = new ChartVisualStateStyle(),
                X = 10,
                Y = 10,
                Width = 20,
                Height = 30,
                CategoryIndex = 0,
                Value = 6
            }
        ]);
    }

    private static PiePayloadCollection BuildPiePayloads()
    {
        return new PiePayloadCollection
        {
            Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(50, 50),
            Radius = 40,
            TotalValue = 10,
            ShowLabels = true,
            ShowPercentages = true,
            Slices =
            [
                new PiePayload
                {
                    ChartId = "chart",
                    GroupId = "group",
                    Id = "slice",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new ChartVisualStateStyle(),
                    ColorIndex = 0,
                    StartAngle = 0,
                    EndAngle = 180,
                    MidAngle = 90,
                    Label = "A",
                    LabelPosition = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(60, 60),
                    Value = 10,
                    IsMultiDonutSlice = false
                }
            ]
        };
    }

    private static DonutPayloadCollection BuildDonutPayloads()
    {
        return new DonutPayloadCollection
        {
            Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(50, 50),
            Radius = 40,
            InnerRadius = 20,
            ShowLabels = true,
            ShowPercentages = false,
            Slices =
            [
                new PiePayload
                {
                    ChartId = "chart",
                    GroupId = "group",
                    Id = "slice-d",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new ChartVisualStateStyle(),
                    ColorIndex = 0,
                    StartAngle = 0,
                    EndAngle = 180,
                    MidAngle = 90,
                    Label = "B",
                    LabelPosition = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(60, 60),
                    Value = 10,
                    IsMultiDonutSlice = false
                }
            ]
        };
    }

    private static MultiDonutPayloadCollection BuildMultiDonutPayloads()
    {
        return new MultiDonutPayloadCollection
        {
            Center = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartPoint(50, 50),
            Rings = [BuildDonutPayloads()]
        };
    }

    private static ClipPathLayer BuildClipPathLayer()
    {
        var payload = new ClipPathPayload
        {
            X = 0,
            Y = 0,
            Width = 100,
            Height = 80
        };

        return new ClipPathLayer(payload);
    }

    private static ChartLayoutPayload BuildLayoutPayload()
    {
        return new ChartLayoutPayload(
            new ChartTitlePayload { Text = "Title", Area = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartRect(0, 0, 100, 20) },
            new ChartSubtitlePayload { Text = "Subtitle", Area = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartRect(0, 20, 100, 20) },
            new ChartLegendPayload
            {
                Area = new FluentUI.Blazor.Community.Components.Charts.Drawing.ChartRect(0, 40, 200, 40),
                ItemCount = 1,
                Shape = ChartLegendItemShape.Circle,
                Items = [new LegendItem("Serie", 0)]
            });
    }

    private static AxesLayer BuildAxesLayer() => new(BuildAxisPayload());

    private static GridLayer BuildGridLayer() => new(BuildGridPayload());

    private sealed class FakeLayer : ILayer
    {
        public string Key => "fake";

        public LayerOrder Order => LayerOrder.Content;

        public int Priority => 0;

        public ILayerPayload LayerPayload { get; } = new FakePayload();
    }

    private sealed class FakePayload : ILayerPayload
    {
    }
}
