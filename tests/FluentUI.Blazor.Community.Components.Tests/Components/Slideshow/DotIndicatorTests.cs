using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class DotIndicatorTests
    : TestBase
{
    public DotIndicatorTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddScoped<DeviceInfoState>();
    }

    [Fact]
    public void AriaLabel_ShouldReflectIndex()
    {
        // Arrange
        var comp = RenderComponent<DotIndicator<string>>(parameters => parameters
            .Add(p => p.Index, 2)
            .Add(p => p.CurrentSlideshowIndex, 1)
            .Add(p => p.Orientation, Orientation.Horizontal)
            .AddCascadingValue(new FluentCxSlideshow<string>())
        );

        // Act
        var span = comp.Find("span");

        // Assert
        Assert.Equal("Item 2", span.GetAttribute("aria-label"));
    }

    [Theory]
    [InlineData(0, 1, Orientation.Horizontal, "dot-indicator dot-indicator-active")]
    [InlineData(0, 2, Orientation.Horizontal, "dot-indicator")]
    [InlineData(0, 1, Orientation.Vertical, "dot-indicator dot-indicator-active dot-indicator-vertical")]
    public void Css_ShouldReflectState(int index, int current, Orientation orientation, string expectedClasses)
    {
        // Arrange
        var comp = RenderComponent<DotIndicator<string>>(parameters => parameters
            .Add(p => p.Index, index)
            .Add(p => p.CurrentSlideshowIndex, current)
            .Add(p => p.Orientation, orientation)
            .AddCascadingValue(new FluentCxSlideshow<string>())
        );

        // Act
        var span = comp.Find("span");

        // Assert
        Assert.Equal(expectedClasses, span.GetAttribute("class"));
    }

    [Fact]
    public async Task MoveToIndexAsync_ShouldCallParent()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Index, 3)
            .Add(p => p.Items, new List<string> { "Item 0", "Item 1", "Item 2", "Item 3", "Item 4" })
        );

        var span = comp.FindAll("span").FirstOrDefault(x => x.GetAttribute("aria-label") == "Item 2");

        // Act
        await span.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // Assert
        Assert.Equal(3, comp.Instance.Index);
    }
}
