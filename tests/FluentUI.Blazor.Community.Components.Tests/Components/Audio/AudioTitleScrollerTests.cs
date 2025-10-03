using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Audio;
public class AudioTitleScrollerTests : TestBase
{
    public AudioTitleScrollerTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void TitleParameter_ShouldRenderCorrectly()
    {
        // Arrange
        var title = "Titre audio";

        // Act
        var cut = RenderComponent<AudioTitleScroller>(parameters => parameters
            .Add(p => p.Title, title)
        );

        Assert.Equal(title, cut.Instance.Title);
        Assert.Contains(title, cut.Markup);
    }
}
