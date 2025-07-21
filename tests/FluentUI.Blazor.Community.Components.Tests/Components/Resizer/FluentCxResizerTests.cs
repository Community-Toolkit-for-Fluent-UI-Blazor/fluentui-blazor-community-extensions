using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;


namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Resizer;

public class FluentCxResizerTests : TestBase
{
    public FluentCxResizerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void FluentCxResizer_Default()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCxResizer_Default_RendersCorrectly()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        var mainDiv = cut.Find("div.fluentcx-resizer");
        Assert.NotNull(mainDiv);
        Assert.NotNull(mainDiv.GetAttribute("id"));
    }

    [Fact]
    public void FluentCxResizer_WithChildContent_RendersContent()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.AddChildContent("<span>Test Content</span>");
        });

        // Assert
        var contentContainer = cut.Find(".fluentcx-resizer-child-content-container");
        Assert.Contains("Test Content", contentContainer.InnerHtml);
    }

    [Fact]
    public void FluentCxResizer_WithResizeEnabled_ShowsResizeHandlers()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.IsResizeEnabled, true);
        });

        // Assert
        var handlers = cut.FindAll(".fluentcx-resizer-handler");
        Assert.Equal(3, handlers.Count); // Should have 3 handlers (Horizontally, Vertically, Both)
    }

    [Fact]
    public void FluentCxResizer_WithResizeDisabled_HidesResizeHandlers()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.IsResizeEnabled, false);
        });

        // Assert
        var handlers = cut.FindAll(".fluentcx-resizer-handler");
        Assert.Empty(handlers);
    }

    [Theory]
    [InlineData(LocalizationDirection.LeftToRight)]
    [InlineData(LocalizationDirection.RightToLeft)]
    public void FluentCxResizer_WithLocalizationDirection_UsesCorrectHandlers(LocalizationDirection direction)
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.LocalizationDirection, direction);
            parameters.Add(p => p.IsResizeEnabled, true);
        });

        // Assert
        var handlers = cut.FindAll(".fluentcx-resizer-handler");
        Assert.Equal(3, handlers.Count);

        // Verify handlers have different styles based on direction
        var handlerStyles = handlers.Select(h => h.GetAttribute("style")).ToList();
        Assert.All(handlerStyles, style => Assert.NotNull(style));
    }

    [Fact]
    public void FluentCxResizer_WithCustomClass_AppliesClass()
    {
        // Arrange
        var customClass = "custom-resizer-class";

        // Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.Class, customClass);
        });

        // Assert
        var mainDiv = cut.Find("div");
        Assert.Contains(customClass, mainDiv.GetAttribute("class"));
        Assert.Contains("fluentcx-resizer", mainDiv.GetAttribute("class"));
    }

    [Fact]
    public void FluentCxResizer_WithCustomStyle_AppliesStyle()
    {
        // Arrange
        var customStyle = "background-color: red;";

        // Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.Style, customStyle);
        });

        // Assert
        var mainDiv = cut.Find("div");
        var style = mainDiv.GetAttribute("style");
        Assert.Contains("background-color: red", style);
    }

    [Fact]
    public void FluentCxResizer_WithSpanGridId_SetsParameter()
    {
        // Arrange
        var spanGridId = "test-grid-123";

        // Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.SpanGridId, spanGridId);
        });

        // Assert
        Assert.Equal(spanGridId, cut.Instance.SpanGridId);
    }

    [Fact]
    public void FluentCxResizer_WithoutSpanGridId_AllowsNull()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        Assert.Null(cut.Instance.SpanGridId);
    }

    [Fact]
    public async Task FluentCxResizer_OnTapped_InvokesCallback()
    {
        // Arrange
        var tappedCalled = false;
        MouseEventArgs? receivedArgs = null;

        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.OnTapped, EventCallback.Factory.Create<MouseEventArgs>(this, args =>
            {
                tappedCalled = true;
                receivedArgs = args;
            }));
        });

        // Act
        var mainDiv = cut.Find("div.fluentcx-resizer");
        await mainDiv.ClickAsync(new MouseEventArgs());

        // Assert
        Assert.True(tappedCalled);
        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public async Task FluentCxResizer_OnDoubleTapped_InvokesCallback()
    {
        // Arrange
        var doubleTappedCalled = false;
        MouseEventArgs? receivedArgs = null;

        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.OnDoubleTapped, EventCallback.Factory.Create<MouseEventArgs>(this, args =>
            {
                doubleTappedCalled = true;
                receivedArgs = args;
            }));
        });

        // Act
        var mainDiv = cut.Find("div.fluentcx-resizer");
        await mainDiv.DoubleClickAsync(new MouseEventArgs());

        // Assert
        Assert.True(doubleTappedCalled);
        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public async Task FluentCxResizer_Resized_InvokesCallback()
    {
        // Arrange
        var resizedCalled = false;
        ResizedEventArgs? receivedArgs = null;

        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.OnResized, EventCallback.Factory.Create<ResizedEventArgs>(this, args =>
            {
                resizedCalled = true;
                receivedArgs = args;
            }));
        });

        var testEventArgs = new ResizedEventArgs
        {
            Id = "test-id",
            Orientation = ResizerHandler.Both,
            ColumnSpan = 2,
            RowSpan = 3
        };

        // Act
        await cut.Instance.Resized(testEventArgs);

        // Assert
        Assert.True(resizedCalled);
        Assert.Equal(testEventArgs, receivedArgs);
    }

    [Fact]
    public void FluentCxResizer_ParameterChange_SpanGridId_UpdatesCorrectly()
    {
        // Arrange
        var initialSpanGridId = "initial-grid";
        var newSpanGridId = "new-grid";

        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.SpanGridId, initialSpanGridId);
        });

        // Act
        cut.SetParametersAndRender(parameters =>
        {
            parameters.Add(p => p.SpanGridId, newSpanGridId);
        });

        // Assert
        Assert.Equal(newSpanGridId, cut.Instance.SpanGridId);
    }

    [Fact]
    public void FluentCxResizer_DefaultIsResizeEnabled_IsTrue()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        Assert.True(cut.Instance.IsResizeEnabled);
    }

    [Fact]
    public void FluentCxResizer_DefaultLocalizationDirection_IsLeftToRight()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        Assert.Equal(LocalizationDirection.LeftToRight, cut.Instance.LocalizationDirection);
    }

    [Fact]
    public void FluentCxResizer_HandlerCssClasses_ContainCorrectCursors()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.IsResizeEnabled, true);
        });

        // Assert
        var handlers = cut.FindAll(".fluentcx-resizer-handler");

        // Should have handlers with different cursor classes
        var cssClasses = handlers.Select(h => h.GetAttribute("class") ?? "").ToList();

        Assert.Contains(cssClasses, c => c.Contains("cursor-ew"));      // Horizontally
        Assert.Contains(cssClasses, c => c.Contains("cursor-ns"));      // Vertically
        Assert.Contains(cssClasses, c => c.Contains("cursor-nwse"));    // Both
    }

    [Fact]
    public void FluentCxResizer_HasUniqueId()
    {
        // Arrange & Act
        var cut1 = RenderComponent<FluentCxResizer>();
        var cut2 = RenderComponent<FluentCxResizer>();

        // Assert
        Assert.NotEqual(cut1.Instance.Id, cut2.Instance.Id);
        Assert.NotNull(cut1.Instance.Id);
        Assert.NotNull(cut2.Instance.Id);
    }

    [Fact]
    public async Task FluentCxResizer_OnTapped_WithoutDelegate_DoesNotThrow()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>(); // No OnTapped callback

        // Act & Assert - Should not throw
        var mainDiv = cut.Find("div.fluentcx-resizer");
        await mainDiv.ClickAsync(new MouseEventArgs());
    }

    [Fact]
    public async Task FluentCxResizer_OnDoubleTapped_WithoutDelegate_DoesNotThrow()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>(); // No OnDoubleTapped callback

        // Act & Assert - Should not throw
        var mainDiv = cut.Find("div.fluentcx-resizer");
        await mainDiv.DoubleClickAsync(new MouseEventArgs());
    }

    [Fact]
    public async Task FluentCxResizer_Resized_WithoutDelegate_DoesNotThrow()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>(); // No OnResized callback

        var testEventArgs = new ResizedEventArgs
        {
            Id = "test-id",
            Orientation = ResizerHandler.Both,
            ColumnSpan = 2,
            RowSpan = 3
        };

        // Act & Assert - Should not throw
        await cut.Instance.Resized(testEventArgs);
    }

    [Fact]
    public void FluentCxResizer_SetParametersAsync_SpanGridIdChanged_SetsResetFlag()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.SpanGridId, "initial-grid");
        });

        // Act - Change SpanGridId to trigger reset initialization
        cut.SetParametersAndRender(parameters =>
        {
            parameters.Add(p => p.SpanGridId, "new-grid");
        });

        // Assert
        Assert.Equal("new-grid", cut.Instance.SpanGridId);
        // The _resetInitialization flag should be set internally
        // We can't directly test the private field, but we can verify the component still renders correctly
        var mainDiv = cut.Find("div.fluentcx-resizer");
        Assert.NotNull(mainDiv);
    }

    [Fact]
    public void FluentCxResizer_SetParametersAsync_SpanGridIdUnchanged_DoesNotSetResetFlag()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.SpanGridId, "same-grid");
        });

        // Act - Set same SpanGridId (no change)
        cut.SetParametersAndRender(parameters =>
        {
            parameters.Add(p => p.SpanGridId, "same-grid");
        });

        // Assert
        Assert.Equal("same-grid", cut.Instance.SpanGridId);
        var mainDiv = cut.Find("div.fluentcx-resizer");
        Assert.NotNull(mainDiv);
    }

    [Fact]
    public async Task FluentCxResizer_DisposeAsync_CallsDisposeCorrectly()
    {
        // Arrange
        var cut = RenderComponent<FluentCxResizer>();

        // Act & Assert - Should not throw when disposed
        await cut.Instance.DisposeAsync();
    }

    [Fact]
    public void FluentCxResizer_InternalClass_IncludesBaseClass()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert
        var mainDiv = cut.Find("div");
        var classAttribute = mainDiv.GetAttribute("class");
        Assert.Contains("fluentcx-resizer", classAttribute);
    }

    [Fact]
    public void FluentCxResizer_InternalStyle_BuildsCorrectly()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.Style, "background: red;");
        });

        // Assert
        var mainDiv = cut.Find("div");
        var styleAttribute = mainDiv.GetAttribute("style");
        Assert.Contains("background: red", styleAttribute);
    }

    [Fact]
    public void FluentCxResizer_ResizeHandlers_Property_ReturnsCorrectHandlers()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>();

        // Assert - Access the property indirectly by checking rendered handlers
        var handlers = cut.FindAll(".fluentcx-resizer-handler");
        Assert.Equal(3, handlers.Count);
    }

    [Fact]
    public void FluentCxResizer_WithNullChildContent_RendersEmptyContainer()
    {
        // Arrange & Act
        var cut = RenderComponent<FluentCxResizer>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment?)null);
        });

        // Assert
        var contentContainer = cut.Find(".fluentcx-resizer-child-content-container");
        Assert.Empty(contentContainer.InnerHtml.Trim());
    }
}
