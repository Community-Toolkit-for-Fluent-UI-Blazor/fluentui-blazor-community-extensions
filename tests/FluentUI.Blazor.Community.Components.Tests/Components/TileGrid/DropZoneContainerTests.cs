using FluentUI.Blazor.Community.Components.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.TileGrid;

public class DropZoneContainerTests : TestBase
{
    public DropZoneContainerTests()
    {
        // Register required DropZoneState<string> for DI directly
        Services.AddScoped<DropZoneState<string>>();
    }

    [Fact]
    public void DropZoneContainer_Default_InitializesCorrectly()
    {
        // Arrange
        var comp = RenderComponent<FluentCxDropZoneContainer<string>>();

        // Act
        var instance = comp.Instance;

        // Assert
        Assert.NotNull(instance);
        Assert.Empty(instance.Items); // Items should be an empty list, not null
        Assert.Null(instance.ItemContent);
        Assert.Null(instance.ItemCss);
        Assert.Null(instance.IsDragAllowed);
        Assert.Null(instance.IsDropAllowed);
    }

    [Fact]
    public void DropZoneContainer_Items_RenderedCorrectly()
    {
        // Arrange
        var items = new List<string> { "A", "B", "C" };
        var comp = RenderComponent<FluentCxDropZoneContainer<string>>(parameters =>
        {
            parameters.Add(p => p.Items, items);
            parameters.Add(p => p.ItemContent, item => builder => builder.AddContent(0, $"Item: {item}"));
        });

        // Act
        var markup = comp.Markup;

        // Assert
        Assert.Contains("Item: A", markup);
        Assert.Contains("Item: B", markup);
        Assert.Contains("Item: C", markup);
    }

    [Fact]
    public void DropZoneContainer_ItemCss_Delegate_Applied()
    {
        // Arrange
        var items = new List<string> { "A" };
        var comp = RenderComponent<FluentCxDropZoneContainer<string>>(parameters =>
        {
            parameters.Add(p => p.Items, items);
            parameters.Add(p => p.ItemCss, item => $"css-{item}");
        });

        // Act
        var instance = comp.Instance;
        var css = instance.ItemCss?.Invoke("A");

        // Assert
        Assert.Equal("css-A", css);
    }

    [Fact]
    public void DropZoneContainer_IsDragAllowed_Delegate_Applied()
    {
        // Arrange
        var items = new List<string> { "A" };
        var comp = RenderComponent<FluentCxDropZoneContainer<string>>(parameters =>
        {
            parameters.Add(p => p.Items, items);
            parameters.Add(p => p.IsDragAllowed, item => item == "A");
        });

        // Act
        var instance = comp.Instance;
        var allowed = instance.IsDragAllowed?.Invoke("A");

        // Assert
        Assert.True(allowed);
    }

    [Fact]
    public void DropZoneContainer_IsDropAllowed_Delegate_Applied()
    {
        // Arrange
        var items = new List<string> { "A", "B" };
        var comp = RenderComponent<FluentCxDropZoneContainer<string>>(parameters =>
        {
            parameters.Add(p => p.Items, items);
            parameters.Add(p => p.IsDropAllowed, (a, b) => a == b);
        });

        // Act
        var instance = comp.Instance;
        var allowed = instance.IsDropAllowed?.Invoke("A", "A");
        var notAllowed = instance.IsDropAllowed?.Invoke("A", "B");

        // Assert
        Assert.True(allowed);
        Assert.False(notAllowed);
    }
}
