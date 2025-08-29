using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowImageTests
    : TestBase
{
    public SlideshowImageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddScoped<DeviceInfoState>();
    }

    [Fact]
    public void Renders_Image_With_Correct_Attributes()
    {
        // Arrange
        var id = "img1";
        var src = "https://example.com/image.jpg";
        var alt = "Description";
        var title = "Titre de l'image";

        // Act
        var cut = RenderComponent<FluentCxSlideshow<string>>(
            p => p.AddChildContent<SlideshowItem<string>>(
                p => p.AddChildContent<SlideshowImage<string>>(p =>
                p.Add(m => m.Id, id)
                 .Add(m => m.Source, src)
                 .Add(m => m.Alt, alt)
                 .Add(m => m.Title, title))
            )
            .Add(m=>m.Width, 600)
            .Add(m=>m.Height,400)
        );

        // Assert
        var img = cut.Find("img");
        Assert.Equal(id, img.Id);
        Assert.Equal(src, img.GetAttribute("src"));
        Assert.Equal(alt, img.GetAttribute("alt"));
        Assert.Equal(title, img.GetAttribute("title"));
        Assert.Equal("lazy", img.GetAttribute("loading"));
    }
}
