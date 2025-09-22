using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;

public class ColorPaletteItemTests : TestBase
{
    public ColorPaletteItemTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
    }

    [Fact]
    public void ColorPaletteItem_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var cut = RenderComponent<ColorPaletteItem>();

        // Assert
        Assert.Equal("transparent", cut.Instance.Color);
        Assert.Equal(32, cut.Instance.Size);
        Assert.False(cut.Instance.IsFocused);
        Assert.False(cut.Instance.IsSelected);
        Assert.False(cut.Instance.IsAnimated);
        Assert.False(cut.Instance.OnClick.HasDelegate);
        Assert.False(cut.Instance.OnReady.HasDelegate);
    }

    [Fact]
    public void ColorPaletteItem_Parameters_AreSetCorrectly()
    {
        // Arrange & Act
        var cut = RenderComponent<ColorPaletteItem>(parameters => parameters
            .Add(p => p.Color, "#FF0000")
            .Add(p => p.Size, 48)
            .Add(p => p.IsFocused, true)
            .Add(p => p.IsSelected, true)
            .Add(p => p.IsAnimated, true)
        );

        // Assert
        Assert.Equal("#FF0000", cut.Instance.Color);
        Assert.Equal(48, cut.Instance.Size);
        Assert.True(cut.Instance.IsFocused);
        Assert.True(cut.Instance.IsSelected);
        Assert.True(cut.Instance.IsAnimated);
    }

    [Fact]
    public async Task ColorPaletteItem_OnReady_IsInvoked_OnFirstRender()
    {
        // Arrange
        ColorPaletteItem received = null;
        var tcs = new TaskCompletionSource();

        var cut = RenderComponent<ColorPaletteItem>(parameters => parameters
            .Add(p => p.OnReady, EventCallback.Factory.Create<ColorPaletteItem>(this, (item) =>
            {
                received = item;
                tcs.SetResult();
            }))
        );

        // Act
        await tcs.Task; // Attend que OnReady soit appelé

        // Assert
        Assert.NotNull(received);
        Assert.Same(cut.Instance, received);
    }

    [Fact]
    public void ColorPaletteItem_OnClick_CanBeSet_AndInvoked()
    {
        // Arrange
        var clicked = false;
        var cut = RenderComponent<ColorPaletteItem>(parameters => parameters
            .Add(p => p.OnClick, EventCallback.Factory.Create(this, () => clicked = true))
        );

        // Act
        cut.Instance.OnClick.InvokeAsync();

        // Assert
        Assert.True(clicked);
    }
}
