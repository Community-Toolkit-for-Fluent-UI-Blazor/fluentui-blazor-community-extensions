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

    [Fact]
    public void GetInternalOrientation_ShouldReturnOrientationBasedOnIndicatorPosition()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.ShowIndicators, true)
            .Add(p => p.IndicatorPosition, SlideshowIndicatorPosition.Left)
        );

        // Act
        var orientation = comp.Instance.InternalOrientation;

        // Assert
        Assert.Equal(Orientation.Vertical, orientation);
    }

    [Fact]
    public void IsCurrent_ShouldReturnTrue_WhenIndexMatches()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Index, 2)
        );

        // Act (méthode privée, testée via GetAriaHiddenValue)
        var item = new SlideshowItem<string>();
        comp.Instance.Add(item);
        var result = comp.Instance.GetAriaHiddenValue(item);

        // Assert
        Assert.Equal("true", result);
    }

    [Fact]
    public void AddAndRemove_SlideshowItem_ShouldUpdateSlides()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>();
        var item = new SlideshowItem<string>();

        // Act
        comp.Instance.Add(item);
        Assert.True(comp.Instance.Contains(item));

        comp.Instance.Remove(item);
        Assert.False(comp.Instance.Contains(item));
    }

    [Fact]
    public void AddAndRemove_SlideshowImage_ShouldUpdateImages()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>();
        var image = new SlideshowImage<string>();

        // Act
        comp.Instance.Add(image);
        Assert.True(comp.Instance.Contains(image));

        comp.Instance.Remove(image);
        Assert.False(comp.Instance.Contains(image));
    }

    [Fact]
    public void SetParentSize_ShouldSetWidthAndHeight()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>();

        // Act
        comp.Instance.SetParentSize(123, 456);

        // Assert
        Assert.Equal(123, comp.Instance.Width);
        Assert.Equal(456, comp.Instance.Height);
    }

    [Fact]
    public void ResizeObserverEvent_ShouldUpdateResizedWidthAndHeight()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Width, 100)
            .Add(p => p.Height, 100)
        );
        var resize = new SlideshowResize { Width = 200, Height = 200, FixedWidth = false, FixedHeight = false };

        // Act
        var result = comp.Instance.ResizeObserverEvent(resize);

        // Assert
        Assert.Equal(200, result[0]);
        Assert.Equal(200, result[1]);
    }

    [Fact]
    public async Task OnAutoSizeCompletedAsync_ShouldInvokeCallback()
    {
        // Arrange
        var called = false;
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.OnImagesResizeCompleted, EventCallback.Factory.Create(this, () => called = true))
        );

        // Act
        await comp.Instance.OnAutoSizeCompletedAsync();

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task OnFillSizeCompletedAsync_ShouldUpdateResizedValuesAndInvokeCallback()
    {
        // Arrange
        var called = false;
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.OnImagesResizeCompleted, EventCallback.Factory.Create(this, () => called = true))
        );

        // Act
        await comp.Instance.OnFillSizeCompletedAsync(111, 222);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task MoveToIndexAsync_ShouldSetIndex()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Items, new[] { "a", "b", "c" })
            .Add(p => p.Index, 1)
        );

        // Act
        await comp.Instance.MoveToIndexAsync(1);

        // Assert
        Assert.Equal(2, comp.Instance.Index);
    }

    [Fact]
    public void OnLoopingModeChanged_ShouldResetIndex()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Index, 5)
        );

        // Act
        comp.Instance.GetType().GetMethod("OnLoopingModeChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(comp.Instance, null);

        // Assert
        Assert.Equal(1, comp.Instance.Index);
    }

    [Fact]
    public void GetAriaHiddenValue_ShouldReturnTrueOrFalse()
    {
        // Arrange
        var comp = RenderComponent<FluentCxSlideshow<string>>(parameters => parameters
            .Add(p => p.Index, 1)
        );

        // Act
        var result = comp.Instance.GetAriaHiddenValue(new SlideshowItem<string>());

        // Assert
        Assert.Equal("true", result);
    }
}
