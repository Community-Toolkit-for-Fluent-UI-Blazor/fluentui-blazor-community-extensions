using System.Collections.Generic;
using System.Reflection;
using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Tests.Verify;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class ResponsiveImageTests : FluentUITestContext
{
    public ResponsiveImageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public async Task ResponsiveImage_AddsAndRemovesSource()
    {
        var content = (RenderFragment)(builder =>
        {
            builder.OpenComponent<ResponsiveImage>(0);
            builder.AddAttribute(1, "Source", "sample.png");
            builder.CloseComponent();
        });

        var cut = Render(builder =>
        {
            builder.OpenComponent<FluentCxPicture>(0);
            builder.AddAttribute(1, "ResponsiveContent", content);
            builder.CloseComponent();
        });

        var picture = cut.FindComponent<FluentCxPicture>();
        var sources = GetResponsiveSources(picture.Instance);
        Assert.Single(sources);

        var responsiveImage = cut.FindComponent<ResponsiveImage>();
        await responsiveImage.Instance.DisposeAsync();

        sources = GetResponsiveSources(picture.Instance);
        Assert.Empty(sources);
    }

    private static List<ResponsiveImage> GetResponsiveSources(FluentCxPicture picture)
    {
        var field = typeof(FluentCxPicture).GetField("_internalResponsiveSources", BindingFlags.NonPublic | BindingFlags.Instance);

        return (List<ResponsiveImage>)field!.GetValue(picture)!;
    }
}
