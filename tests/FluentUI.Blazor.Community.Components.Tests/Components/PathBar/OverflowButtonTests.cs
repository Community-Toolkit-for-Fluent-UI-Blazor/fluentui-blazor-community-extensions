using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.PathBar;

public class OverflowButtonTests : TestBase
{
    public OverflowButtonTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    private IRenderedFragment RenderOverflowButton(Action<ComponentParameterCollectionBuilder<OverflowButton>>? configureParameters = null)
    {
        return Render(b =>
        {
            b.OpenComponent<FluentMenuProvider>(0);
            b.CloseComponent();

            var mockPathBar = new Mock<FluentCxPathBar>();
            b.OpenComponent<CascadingValue<FluentCxPathBar>>(1);
            b.AddAttribute(2, "Value", mockPathBar.Object);
            b.AddAttribute(3, "IsFixed", true);
            b.AddAttribute(4, "ChildContent", (RenderFragment)(builder =>
            {
                builder.OpenComponent<OverflowButton>(5);

                if (configureParameters != null)
                {
                    var i = 6;
                    var parameters = new ComponentParameterCollectionBuilder<OverflowButton>();
                    configureParameters(parameters);
                    foreach (var param in parameters.Build())
                    {
                        builder.AddAttribute(i, param.Name, param.Value);
                        i++;
                    }
                }

                builder.CloseComponent();
            }));
            b.CloseComponent();
        });
    }

    private static PathBarItem CreateTestItem(string id, string label)
    {
        return new PathBarItem { Id = id, Label = label };
    }

    [Fact]
    public void OverflowButton_Default_RendersCorrectly()
    {
        // Act
        var cut = RenderOverflowButton();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void OverflowButton_WithEmptyItems_RendersCorrectly()
    {
        // Act
        var cut = RenderOverflowButton(p => p.Add(x => x.Items, new List<IPathBarItem>()));

        // Assert
        cut.Verify();
    }

    [Fact]
    public void OverflowButton_WithSingleItem_RendersCorrectly()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1")
        };

        // Act
        var cut = RenderOverflowButton(p => p.Add(x => x.Items, items));

        // Assert
        cut.Verify();
    }

    [Fact]
    public void OverflowButton_WithMultipleItems_RendersCorrectly()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1"),
            CreateTestItem("item2", "Item 2"),
            CreateTestItem("item3", "Item 3")
        };

        // Act
        var cut = RenderOverflowButton(p => p.Add(x => x.Items, items));

        // Assert
        cut.Verify();
    }


    [Fact]
    public void OverflowButton_ClickButton_OpensMenu()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1")
        };

        var cut = RenderOverflowButton(p => p.Add(x => x.Items, items));
        var button = cut.Find("fluent-button");

        // Act
        button.Click();

        // Assert
        cut.Verify();
    }

    [Fact]
    public void OverflowButton_ClickButton_ThenClickAgain_ClosesMenu()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1")
        };

        var cut = RenderOverflowButton(p => p.Add(x => x.Items, items));
        var button = cut.Find("fluent-button");

        // Act
        button.Click(); // Open menu
        button.Click(); // Close menu

        // Assert
        cut.Verify();
    }

    [Fact]
    public void OverflowButton_WithNullParent_MenuItemClick_DoesNotThrow()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1")
        };

        var cut = Render(b =>
        {
            b.OpenComponent<FluentMenuProvider>(0);
            b.CloseComponent();

            b.OpenComponent<OverflowButton>(1);
            b.AddAttribute(2, "Items", items);
            b.CloseComponent();
        });

        var button = cut.Find("fluent-button");
        button.Click(); // Open menu

        // Act & Assert - Should not throw
        var menuItem = cut.Find("fluent-menu-item");
        menuItem.Click();
    }

    [Fact]
    public void OverflowButton_HasCorrectButtonProperties()
    {
        // Arrange
        var items = new List<IPathBarItem>
        {
            CreateTestItem("item1", "Item 1")
        };

        // Act
        var cut = RenderOverflowButton(p =>
        {
            p.Add(x => x.Items, items);
            p.Add(x => x.Id, "test-overflow");
        });

        // Assert
        var button = cut.Find("fluent-button");
        Assert.Equal("Buttontest-overflow", button.GetAttribute("id"));
        Assert.Equal("stealth", button.GetAttribute("appearance"));
        cut.Verify();
    }
}
