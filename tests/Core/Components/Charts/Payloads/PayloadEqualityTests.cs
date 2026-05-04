using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using Xunit;

namespace Components.Tests.Components.Charts.Payloads;

public class PayloadEqualityTests
{
    [Fact]
    public void BarPayload_Equality_UsesValues()
    {
        var first = new BarPayload
        {
            Id = "bar",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 5
        };
        var second = new BarPayload
        {
            Id = "bar",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 5
        };

        Assert.Equal(first, second);
    }

    [Fact]
    public void ColumnPayload_Equality_UsesValues()
    {
        var first = new ColumnPayload
        {
            Id = "col",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 5
        };
        var second = new ColumnPayload
        {
            Id = "col",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4,
            CategoryIndex = 0,
            Value = 5
        };

        Assert.Equal(first, second);
    }

    [Fact]
    public void LinePayload_Equality_UsesLinePointValues()
    {
        var path = new LinePathPayload
        {
            Id = "path",
            Smooth = false,
            Points = [new ChartPoint(0, 0)]
        };

        var first = new LinePointPayload
        {
            Id = "pt",
            GroupId = "group",
            ChartId = "chart",
            Index = 0,
            SerieIndex = 0,
            Normal = new ChartVisualStateStyle(),
            X = 0,
            Y = 0,
            Value = 1,
            CategoryIndex = 0
        };

        var second = first with { };

        Assert.Equal(first, second);
        Assert.Equal(path, path);
    }
}
