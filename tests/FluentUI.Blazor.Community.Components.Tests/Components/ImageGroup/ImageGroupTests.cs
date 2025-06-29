using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ImageGroup;

public class ImageGroupTests : TestBase
{
    public ImageGroupTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void FluentCxImageGroup_Default()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>();

        // Act

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCxImageGroup_WithItems_LessThenVisible()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 2; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        Assert.Equal(2, cut.FindAll("img").Count);
        Assert.Empty(cut.FindAll("fluent-button"));
    }

    [Fact]
    public void FluentCxImageGroup_WithItems_MoreThenVisible()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 5; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        Assert.Equal(3, cut.FindAll("img").Count);
        Assert.Single(cut.FindAll("fluent-button"));
    }

    [Fact]
    public void FluentCxImageGroup_SpreadLayout()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.GroupLayout, ImageGroupLayout.Spread);
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 5; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        // Margin-left should be applied twice
        Assert.Equal(2, cut.FindAll("img[style*='margin-left: 16px']").Count);
        Assert.Equal(3, cut.FindAll("img").Count);
        Assert.Single(cut.FindAll("fluent-button"));
    }

    [Fact]
    public void FluentCxImageGroup_StackLayout()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.Id, "test-image-group");
            parameters.Add(p => p.GroupLayout, ImageGroupLayout.Stack);
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 5; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        // Margin-left should be applied twice
        Assert.Equal(2, cut.FindAll("img[style*='margin-left: -15px']").Count);
        Assert.Equal(3, cut.FindAll("img").Count);
        Assert.Single(cut.FindAll("fluent-button"));
    }

    [Theory]
    [InlineData(ImageSize.Size20)]
    [InlineData(ImageSize.Size24)]
    [InlineData(ImageSize.Size28)]
    public void FluentCxImageGroup_ImageSize(ImageSize size)
    {
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.Size, size);
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 3; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify(suffix: size.ToString());
        var images = cut.FindAll("img");
        Assert.Equal(3, images.Count);
        foreach (var image in images)
        {
            var style = image.GetAttribute("style");
            Assert.Contains($"width: {(int)size}px", style);
            Assert.Contains($"height: {(int)size}px", style);
        }
    }

    [Fact]
    public void FluentCxImageGroup_WithBorderStyle()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.BorderStyle, "1px solid red");
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 3; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        var images = cut.FindAll("img");
        foreach (var image in images)
        {
            var style = image.GetAttribute("style");
            Assert.Contains("border: 1px solid red", style);
        }
    }

    [Fact]
    public void FluentCxImageGroup_WithBackgroundStyle()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.BackgroundStyle, "blue");
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 3; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify();
        var images = cut.FindAll("img");
        foreach (var image in images)
        {
            var style = image.GetAttribute("style");
            Assert.Contains("background-color: blue", style);
        }
    }

    [Theory]
    [InlineData(ImageShape.Circle)]
    [InlineData(ImageShape.Square)]
    [InlineData(ImageShape.RoundSquare)]
    public void FluentCxImageGroup_ImageShape(ImageShape shape)
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters =>
        {
            parameters.Add(p => p.Shape, shape);
            parameters.AddChildContent(builder =>
            {
                for (var i = 0; i < 3; i++)
                {
                    builder.OpenComponent<FluentCxImageGroupItem>(0);
                    builder.CloseComponent();
                }
            });
        });

        // Act

        // Assert
        cut.Verify(suffix: shape.ToString());
        var images = cut.FindAll("img");
        foreach (var image in images)
        {
            var style = image.GetAttribute("style");
            Assert.Contains($"border-radius: {shape.ToBorderRadius()}", style);
        }
    }
}
