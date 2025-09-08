using System;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ArtisticScatter;

public class ArtisticScatterItemTests : TestBase
{
    public ArtisticScatterItemTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void OnInitialized_Throws_If_Parent_Is_Null()
    {
        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => RenderComponent<ArtisticScatterItem>());
        Assert.Contains("ArtisticScatterItem can only be used", ex.Message);
    }

    [Fact]
    public void OnInitialized_Sets_Fields_And_Adds_To_Parent()
    {
        var comp = RenderComponent<FluentCxArtisticScatter>(p =>
         p.AddChildContent<ArtisticScatterItem>());

        Assert.Equal(1, comp.Instance.DisplayedItemCount);
    }

    [Fact]
    public async Task Dispose_Removes_From_Parent_And_Suppresses_Finalize()
    {
        var comp = RenderComponent<FluentCxArtisticScatter>(p =>
         p.AddChildContent<ArtisticScatterItem>());

        Assert.Equal(1, comp.Instance.DisplayedItemCount);
        var item = comp.FindComponent<ArtisticScatterItem>();
        await item.Instance.DisposeAsync();
        Assert.Equal(0, comp.Instance.DisplayedItemCount);
    }
}
