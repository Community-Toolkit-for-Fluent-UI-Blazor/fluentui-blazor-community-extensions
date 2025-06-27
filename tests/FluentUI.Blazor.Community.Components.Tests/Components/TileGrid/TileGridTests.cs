using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Components.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.TileGrid;

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
}
