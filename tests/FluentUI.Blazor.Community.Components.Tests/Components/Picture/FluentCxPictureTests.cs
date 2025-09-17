using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Picture;

public class FluentCxPictureTests : TestContext
{
    [Fact]
    public void RendersCaption_WhenCaptionIsSet()
    {
        var cut = RenderComponent<FluentCxPicture>(parameters => parameters
            .Add(p => p.Caption, "Test Caption")
        );

        cut.Find("figcaption").MarkupMatches("<figcaption>Test Caption</figcaption>");
    }

    [Fact]
    public void RendersResponsiveImages_WhenResponsiveContentSet()
    {
        var cut = RenderComponent<FluentCxPicture>(parameters => parameters
            .Add(p => p.ResponsiveContent, builder =>
            {
                builder.OpenComponent<ResponsiveImage>(0);
                builder.AddAttribute(1, "Source", "img-small.png");
                builder.AddAttribute(2, "Media", "(max-width: 600px)");
                builder.CloseComponent();

                builder.OpenComponent<ResponsiveImage>(3);
                builder.AddAttribute(4, "Source", "img-large.png");
                builder.AddAttribute(5, "Media", "(min-width: 601px)");
                builder.CloseComponent();
            })
        );

        Assert.Equal(2, cut.FindAll("source").Count);
    }

    [Fact]
    public void DoesNotRenderCaption_WhenCaptionIsNullOrEmpty()
    {
        var cut = RenderComponent<FluentCxPicture>(parameters => parameters
            .Add(p => p.Caption, null)
        );

        Assert.Empty(cut.FindAll("figcaption"));
    }

    [Fact]
    public void RendersOverlayContent_WhenOverlayContentIsSet()
    {
        var cut = RenderComponent<FluentCxPicture>(parameters => parameters
            .Add(p => p.OverlayContent, (builder) =>
            {
                builder.AddContent(0, "Overlay");
            })
        );

        cut.Markup.Contains("Overlay");
    }

    [Fact]
    public void RendersImgWithCorrectAttributes()
    {
        var cut = RenderComponent<FluentCxPicture>(parameters => parameters
            .Add(p => p.Source, "img.png")
            .Add(p => p.Alt, "alt text")
            .Add(p => p.Title, "title text")
            .Add(p => p.Language, "fr")
            .Add(p => p.Class, "custom-class")
            .Add(p => p.Style, "width:100px;")
        );

        var img = cut.Find("img");
        Assert.Equal("img.png", img.GetAttribute("src"));
        Assert.Equal("alt text", img.GetAttribute("alt"));
        Assert.Equal("title text", img.GetAttribute("title"));
        Assert.Equal("fr", img.GetAttribute("lang"));
        Assert.Contains("custom-class", img.GetAttribute("class"));
        Assert.Contains("width:100px;", img.GetAttribute("style"));
    }
}
