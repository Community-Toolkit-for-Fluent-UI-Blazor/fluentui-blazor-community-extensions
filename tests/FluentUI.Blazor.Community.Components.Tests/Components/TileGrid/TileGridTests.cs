using System.Globalization;
using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.TileGrid;

public class TileGridTests
    : TestBase
{
    public TileGridTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<DropZoneState<NoFileEntryData>>();
    }

    [Fact]
    public void FluentCxTileGrid_Default()
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>();

        // Act

        // Asset
        comp.Verify();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(3)]
    [InlineData(2)]
    [InlineData(null)]
    public void FluentCxTileGrid_Columns(int? columns)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Columns, columns);
        });

        // Act

        // Assert
        comp.Verify(suffix: columns?.ToString(CultureInfo.InvariantCulture));
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"repeat({(columns.HasValue ? columns : "auto-fit")}", style);
    }

    [Theory]
    [InlineData("1fr")]
    [InlineData("50px")]
    [InlineData("6em")]
    public void FluentCxTileGrid_ColumnWidth(string columnWidth)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.ColumnWidth, columnWidth);
        });

        // Act

        // Assert
        comp.Verify(suffix: columnWidth);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"minmax(0px, {columnWidth})", style);
    }

    [Theory]
    [InlineData("1fr")]
    [InlineData("50px")]
    [InlineData("6em")]
    public void FluentCxTileGrid_MinimumColumnWidth(string? columnWidth)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.MinimumColumnWidth, columnWidth);
        });

        // Act

        // Assert
        comp.Verify(suffix: columnWidth);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"minmax({columnWidth}, 1fr)", style);
    }

    [Theory]
    [InlineData("150px")]
    [InlineData("80em")]
    [InlineData("200")]
    public void FluentCxTileGrid_RowHeight(string? rowHeight)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.RowHeight, rowHeight);
        });

        // Act

        // Assert
        comp.Verify(suffix: rowHeight);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"minmax(0px, {rowHeight})", style);
    }

    [Theory]
    [InlineData("150px")]
    [InlineData("80em")]
    [InlineData("200")]
    public void FluentCxTileGrid_MinimumRowHeight(string? minimumRowHeight)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.MinimumRowHeight, minimumRowHeight);
        });

        // Act

        // Assert
        comp.Verify(suffix: minimumRowHeight);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"minmax({minimumRowHeight},", style);
    }

    [Theory]
    [InlineData("150px")]
    [InlineData("80em")]
    [InlineData("200")]
    public void FluentCxTileGrid_Width(string? width)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Width, width);
        });

        // Act

        // Assert
        comp.Verify(suffix: width);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"width: {width}", style);
    }

    [Theory]
    [InlineData("150px")]
    [InlineData("80em")]
    [InlineData("200")]
    public void FluentCxTileGrid_Height(string? height)
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Height, height);
        });

        // Act

        // Assert
        comp.Verify(suffix: height);
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"height: {height}", style);
    }

    [Fact]
    public void FluentCxTileGrid_CanOverflow()
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.CanOverflow, true);
        });

        // Act

        // Assert
        comp.Verify(suffix: "true");
        var divContainer = comp.Find("div");
        var style = divContainer.GetAttribute("style");
        Assert.Contains($"overflow-y: auto", style);
    }

    [Fact]
    public void FluentCxTileGrid_CanResize_Parameter_CanBeSet()
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.CanResize, true);
        });

        // Act
        var instance = comp.Instance;

        // Assert
        Assert.True(instance.CanResize);
    }

    [Fact]
    public void FluentCxTileGrid_CloneItem_Delegate_PassedToDropZoneContainer()
    {
        // Arrange
        var called = false;
        var input = new NoFileEntryData();
        var output = new NoFileEntryData();
        Func<NoFileEntryData, NoFileEntryData> clone = x =>
        {
            called = true;
            return output;
        };
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.CloneItem, clone);
            parameters.Add(p => p.Items, new List<NoFileEntryData> { input });
        });

        // Act
        var dropZone = comp.FindComponent<FluentCxDropZoneContainer<NoFileEntryData>>();
        var result = dropZone.Instance.CloneItem?.Invoke(input);

        // Assert
        Assert.True(called);
        Assert.Same(output, result);
    }

    [Fact]
    public void FluentCxTileGrid_IsDragAllowed_Delegate_PassedToDropZoneContainer()
    {
        // Arrange
        var called = false;
        var input = new NoFileEntryData();
        Func<NoFileEntryData, bool> isDragAllowed = x =>
        {
            called = true;
            return true;
        };
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.IsDragAllowed, isDragAllowed);
            parameters.Add(p => p.Items, new List<NoFileEntryData> { input });
        });

        // Act
        var dropZone = comp.FindComponent<FluentCxDropZoneContainer<NoFileEntryData>>();
        var result = dropZone.Instance.IsDragAllowed?.Invoke(input);

        // Assert
        Assert.True(called);
        Assert.True(result);
    }

    [Fact]
    public void FluentCxTileGrid_IsDropAllowed_Delegate_PassedToDropZoneContainer()
    {
        // Arrange
        var called = false;
        var input1 = new NoFileEntryData();
        var input2 = new NoFileEntryData();
        Func<NoFileEntryData?, NoFileEntryData?, bool> isDropAllowed = (a, b) =>
        {
            called = true;
            return a == b;
        };
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.IsDropAllowed, isDropAllowed);
            parameters.Add(p => p.Items, new List<NoFileEntryData> { input1, input2 });
        });

        // Act
        var dropZone = comp.FindComponent<FluentCxDropZoneContainer<NoFileEntryData>>();
        var result = dropZone.Instance.IsDropAllowed?.Invoke(input1, input1);

        // Assert
        Assert.True(called);
        Assert.True(result);
    }

    [Fact]
    public void FluentCxTileGrid_ItemCss_Delegate_PassedToDropZoneContainer()
    {
        // Arrange
        var called = false;
        var input = new NoFileEntryData();
        Func<NoFileEntryData, string> itemCss = x =>
        {
            called = true;
            return "my-css";
        };
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.ItemCss, itemCss);
            parameters.Add(p => p.Items, new List<NoFileEntryData> { input });
        });

        // Act
        var dropZone = comp.FindComponent<FluentCxDropZoneContainer<NoFileEntryData>>();
        var result = dropZone.Instance.ItemCss?.Invoke(input);

        // Assert
        Assert.True(called);
        Assert.Equal("my-css", result);
    }

    [Fact]
    public void FluentCxTileGrid_ItemContent_RendersCustomContent()
    {
        // Arrange
        var item = new NoFileEntryData();
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Items, new List<NoFileEntryData> { item });
            parameters.Add(p => p.ItemContent, i => builder => builder.AddContent(0, "Custom Item Content"));
        });

        // Act
        var content = comp.Markup;

        // Assert
        Assert.Contains("Custom Item Content", content);
    }

    [Fact]
    public void FluentCxTileGrid_ChildContent_RendersCustomContent()
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.ChildContent, builder => builder.AddContent(0, "Grid Child Content"));
        });

        // Act
        var content = comp.Markup;

        // Assert
        Assert.Contains("Grid Child Content", content);
    }

    [Fact]
    public void FluentCxTileGrid_Items_RendersCorrectNumberOfItems()
    {
        // Arrange
        var items = new List<NoFileEntryData> { new(), new(), new() };
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Items, items);
        });

        // Act
        var dropZone = comp.FindComponent<FluentCxDropZoneContainer<NoFileEntryData>>();
        // The drop zone container should receive the items list
        Assert.Equal(items, dropZone.Instance.Items);
    }

    [Fact]
    public void FluentCxTileGrid_NullParameters_DoesNotThrow()
    {
        // Arrange/Act/Assert
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Items, null);
            parameters.Add(p => p.CloneItem, null);
            parameters.Add(p => p.IsDragAllowed, null);
            parameters.Add(p => p.IsDropAllowed, null);
            parameters.Add(p => p.ItemCss, null);
            // Do not add ItemContent or ChildContent as null due to ambiguity
        });
        Assert.NotNull(comp);
    }

    [Fact]
    public void FluentCxTileGrid_DropContainer_IsSetAfterRender()
    {
        // Arrange
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>();

        // Act
        var instance = comp.Instance;

        // Assert
        Assert.NotNull(instance._dropContainer);
    }

    [Fact]
    public void FluentCxTileGrid_ItemContent_NotRendered_WhenItemsEmpty()
    {
        // Arrange: Items is empty, ItemContent is set
        var comp = RenderComponent<FluentCxTileGrid<NoFileEntryData>>(parameters =>
        {
            parameters.Add(p => p.Items, new List<NoFileEntryData>()); // empty list
            parameters.Add(p => p.ItemContent, i => builder => builder.AddContent(0, "Custom Item Content"));
        });

        // Act
        var content = comp.Markup;

        // Assert: ItemContent should not be rendered
        Assert.DoesNotContain("Custom Item Content", content);
    }
}
