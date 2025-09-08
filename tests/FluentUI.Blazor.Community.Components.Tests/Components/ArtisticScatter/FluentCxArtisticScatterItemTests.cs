using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ArtisticScatter;
public class FluentCxArtisticScatterItemTests : TestBase
{
    public FluentCxArtisticScatterItemTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }


    [Fact]
    public void Component_Should_Render_Correctly()
    {
        // Act
        var cut = RenderComponent<FluentCxArtisticScatter>();

        // Assert
        cut.Markup.Contains(@"class=\""fluent-cx-artistic-scatter\""");
    }

    [Fact]
    public void Component_With_Items_Below_MaxItems_Should_Render_Correctly()
    {
        // Act
        var cut = RenderComponent<FluentCxArtisticScatter>(p =>
            p.Add(p => p.ChildContent, (RenderFragment)(__builder =>
            {
                __builder.OpenComponent<ArtisticScatterItem>(0);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(1);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(2);
                __builder.CloseComponent();
            })));

        // Assert
        var count = cut.FindComponents<ArtisticScatterItem>().Count;

        Assert.Equal(3, count);
    }

    [Fact]
    public void Component_With_Items_Above_MaxItems_Should_Render_Correctly()
    {
        // Act
        var cut = RenderComponent<FluentCxArtisticScatter>(p =>
            p.Add(p => p.ChildContent, (RenderFragment)(__builder =>
            {
                __builder.OpenComponent<ArtisticScatterItem>(0);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(1);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(2);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(4);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(5);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(6);
                __builder.CloseComponent();

                __builder.OpenComponent<ArtisticScatterItem>(7);
                __builder.CloseComponent();
            })));

        // Assert
        var count = cut.FindComponents<ArtisticScatterItem>().Count;

        Assert.Equal(7, count);
        Assert.Equal(5, cut.Instance.DisplayedItemCount);
    }

    [Fact]
    public void OnClickAsync_Should_Be_Called_When_Clicked()
    {
        // Arrange
        var clicked = false;
        var cut = RenderComponent<FluentCxArtisticScatter>(parameters => parameters
            .Add(p => p.OnClick, EventCallback.Factory.Create(this, () =>
            {
                clicked = true;
                return Task.CompletedTask;
            }))
        );

        // Act
        cut.Find("div").Click();

        // Assert
        Assert.True(clicked);
    }
}
