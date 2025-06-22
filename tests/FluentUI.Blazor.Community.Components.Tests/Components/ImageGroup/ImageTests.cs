using Bunit;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.ImageGroup;

public class ImageTests : TestBase
{
    public ImageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void FluentCxImage_NoParent_Throws()
    {
        // Arrange
        Assert.Throws<NullReferenceException>(() => RenderComponent<FluentCxImage>());
    }

    [Fact]
    public void FluentCxImage_WithParent_Default()
    {
        // Arrange
        var cut = RenderComponent<FluentCxImageGroup>(parameters => parameters
            .Add(p => p.MaxVisibleItems, 1)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<FluentCxImage>(0);
                builder.CloseComponent();
            })
        );

        // Act

        // Assert
        cut.Verify();
    }


}
