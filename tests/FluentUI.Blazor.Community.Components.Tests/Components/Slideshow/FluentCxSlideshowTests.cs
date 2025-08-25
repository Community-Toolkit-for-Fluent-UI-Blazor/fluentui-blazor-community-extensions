using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class FluentCxSlideshowTests : TestBase
{
    private class TestItem
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public FluentCxSlideshowTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddScoped<DeviceInfoState>();
    }

    [Fact]
    public void Renders_With_ChildContent()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .AddChildContent("<div>Slide Content</div>")
            .Add(p => p.Width, 400)
            .Add(p => p.Height, 300)
        );

        // Assert
        Assert.Contains("<div>Slide Content</div>", cut.Markup);
    }

    [Fact]
    public void Renders_ItemTemplate_For_Items()
    {
        // Arrange
        var items = new TestItem[] {
            new TestItem()
            {
                Name = "A",
                Id = 1
            },
            new TestItem()
            {
                Name = "B",
                Id = 2
            }
        };

        RenderFragment<TestItem> template = item => builder =>
        {
            builder.AddContent(0, $"Item: {item.Name}");
        };

        // Act
        var cut = RenderComponent<FluentCxSlideshow<TestItem>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.ItemTemplate, template)
            .Add(p => p.ItemFunc, item => item.Id)
            .Add(p => p.Width, 400)
            .Add(p => p.Height, 300)
        );

        // Assert
        Assert.Contains("Item: A", cut.Markup);
        Assert.Contains("Item: B", cut.Markup);
    }

    [Fact]
    public void Shows_Controls_When_Enabled()
    {
        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.ShowControls, true)
        );

        // Assert
        var buttons = cut.FindAll("fluent-button");
        Assert.Equal(2, buttons.Count);
    }

    [Fact]
    public void Shows_Indicators_When_Enabled()
    {
        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.ShowIndicators, true)
        );

        // Assert
        Assert.Contains("slideshow-indicators", cut.Markup);
    }

    [Fact]
    public void Custom_IndicatorTemplate_Is_Used()
    {
        // Arrange
        RenderFragment<int> indicatorTemplate = i => builder =>
        {
            builder.AddContent(0, $"CustomIndicator-{i}");
        };

        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.IndicatorTemplate, indicatorTemplate)
            .Add(p => p.ShowIndicators, true)
            .Add(p => p.Items, new[] { "A", "B" })
        );

        // Assert
        Assert.Contains("CustomIndicator-0", cut.Markup);
        Assert.Contains("CustomIndicator-1", cut.Markup);
    }
}
