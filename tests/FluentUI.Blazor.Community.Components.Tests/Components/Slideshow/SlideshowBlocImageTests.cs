using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowBlocImageTests : TestBase
{
    public SlideshowBlocImageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
        Services.AddScoped<DeviceInfoState>();
    }

    [Fact]
    public void Renders_InternalStyle_With_Background_And_BorderRadius()
    {
        // Arrange
        var bg = "#ff0000";
        var radius = 12;

        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(p => p
        .AddChildContent<SlideshowItem<string>>(p => p
        .AddChildContent<SlideshowBlocImage<string>>(p => p
        .Add(m => m.BackgroundColor, bg)
        .Add(m => m.BorderRadius, radius)
        )
        )
        .Add(m => m.Width, 600)
        .Add(m => m.Height, 400)
        );

        // Assert
        var root = cut.Find(".slideshow-bloc-image");
        var style = root.GetAttribute("style") ?? string.Empty;
        Assert.Contains("background-color", style);
        Assert.Contains(bg, style);
        Assert.Contains("border-radius", style);
        Assert.Contains($"{radius}px", style);
    }

    [Fact]
    public void Renders_Image_With_ImageStyle_Width_Height_And_BorderRadius_And_Alt_Title()
    {
        // Arrange
        var src = "https://example.com/pic.png";
        var alt = "alt text";
        var title = "the title";
        var imgWidth = 100;
        var imgHeight = 200;
        var imgBorder = "50%";

        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(p => p
        .AddChildContent<SlideshowItem<string>>(p => p
        .AddChildContent<SlideshowBlocImage<string>>(p => p
        .Add(m => m.Source, src)
        .Add(m => m.AltText, alt)
        .Add(m => m.Title, title)
        .Add(m => m.ImageWidth, imgWidth)
        .Add(m => m.ImageHeight, imgHeight)
        .Add(m => m.ImageBorderRadius, imgBorder)
        )
        )
        .Add(m => m.Width, 600)
        .Add(m => m.Height, 400)
        );

        // Assert
        var img = cut.Find("img");
        Assert.Equal(src, img.GetAttribute("src"));
        Assert.Equal(alt, img.GetAttribute("alt"));
        Assert.Equal(title, img.GetAttribute("title"));

        // Width/height attributes (when both provided) should be present
        Assert.Equal(imgWidth.ToString(), img.GetAttribute("width"));
        Assert.Equal(imgHeight.ToString(), img.GetAttribute("height"));

        // Style applied from InternalImageStyle should contain pixel sizes and border-radius
        var style = img.GetAttribute("style") ?? string.Empty;
        Assert.Contains($"width: {imgWidth}px", style.Replace("\u00A0", " "));
        Assert.Contains($"height: {imgHeight}px", style.Replace("\u00A0", " "));
        Assert.Contains("border-radius", style);
        Assert.Contains(imgBorder, style);
    }

    [Fact]
    public void Renders_Image_When_Position_Is_Right()
    {
        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(p => p
        .AddChildContent<SlideshowItem<string>>(p => p
        .AddChildContent<SlideshowBlocImage<string>>(p => p
        .Add(m => m.ImagePosition, SlideshowImagePosition.Right)
        .AddChildContent("<p class='child'>child</p>")
        )
        )
        .Add(m => m.Width, 600)
        .Add(m => m.Height, 400)
        );

        // Assert
        var img = cut.Find("img");
        Assert.NotNull(img);
        var child = cut.Find(".slideshow-bloc-image-child-content .child");
        Assert.Equal("child", child.TextContent.Trim());
    }
}
