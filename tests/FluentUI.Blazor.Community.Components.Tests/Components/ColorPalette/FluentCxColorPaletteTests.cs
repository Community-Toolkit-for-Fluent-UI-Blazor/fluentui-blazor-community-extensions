using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;

public class FluentCxColorPaletteTests
    : TestBase
{
    public FluentCxColorPaletteTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Component_Should_Render_Default()
    {
        // Act
        var cut = RenderComponent<FluentCxColorPalette>();

        // Assert
        cut.MarkupMatches(cut.Markup); // Vérifie que le composant se rend sans erreur
    }

    [Fact]
    public async Task GenerateColorsAsync_Should_Generate_ProvidedColors()
    {
        var providedColors = new List<string> { "#123456", "#abcdef" };
        var cut = RenderComponent<FluentCxColorPalette>(parameters => parameters
            .Add(p => p.Mode, ColorPaletteMode.Provided)
            .Add(p => p.ProvidedColors, providedColors)
        );

        await (Task)cut.Instance.GetType()
            .GetMethod("GenerateColorsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null)!;

        var colorsField = cut.Instance.GetType().GetField("_colors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var colors = (List<string>)colorsField!.GetValue(cut.Instance)!;
        Assert.Contains("#123456", colors);
        Assert.Contains("#ABCDEF", colors); // Warning : Colors are converted to uppercase
    }

    [Fact]
    public async Task ToggleSelectAsync_Should_Select_Color_Single()
    {
        var cut = RenderComponent<FluentCxColorPalette>(parameters => parameters
            .Add(p => p.MultiSelect, false)
            .Add(p => p.ProvidedColors, new List<string> { "#111111", "#222222" })
        );

        await (Task)cut.Instance.GetType()
            .GetMethod("GenerateColorsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null)!;

        await (Task)cut.Instance.GetType()
            .GetMethod("ToggleSelectAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, new object[] { "#111111", 0 })!;

        Assert.Equal("#111111", cut.Instance.SelectedColor);
    }

    [Fact]
    public async Task ToggleSelectAsync_Should_Select_Color_Multi()
    {
        var cut = RenderComponent<FluentCxColorPalette>(parameters => parameters
            .Add(p => p.MultiSelect, true)
            .Add(p => p.ProvidedColors, new List<string> { "#111111", "#222222" })
        );

        await (Task)cut.Instance.GetType()
            .GetMethod("GenerateColorsAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, null)!;

        await (Task)cut.Instance.GetType()
            .GetMethod("ToggleSelectAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .Invoke(cut.Instance, new object[] { "#111111", 0 })!;

        Assert.Contains("#111111", cut.Instance.SelectedColors);
    }

    [Theory]
    [InlineData(0, 2, 1)]
    [InlineData(1, 2, 0)]
    public void MoveRight_Should_Move_Correctly(int index, int columns, int expected)
    {
        var cut = RenderComponent<FluentCxColorPalette>(parameters => parameters
            .Add(p => p.Columns, columns)
            .Add(p => p.ProvidedColors, new List<string> { "#111111", "#222222" })
        );

        var method = cut.Instance.GetType().GetMethod("MoveRight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var colorsField = cut.Instance.GetType().GetField("_colors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        colorsField!.SetValue(cut.Instance, new List<string> { "#111111", "#222222" });

        var result = (int)method.Invoke(cut.Instance, new object[] { index })!;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 2, 0)]
    [InlineData(0, 2, 1)]
    public void MoveLeft_Should_Move_Correctly(int index, int columns, int expected)
    {
        var cut = RenderComponent<FluentCxColorPalette>(parameters => parameters
            .Add(p => p.Columns, columns)
            .Add(p => p.ProvidedColors, new List<string> { "#111111", "#222222" })
        );

        var method = cut.Instance.GetType().GetMethod("MoveLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var colorsField = cut.Instance.GetType().GetField("_colors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        colorsField!.SetValue(cut.Instance, new List<string> { "#111111", "#222222" });

        var result = (int)method.Invoke(cut.Instance, new object[] { index })!;
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DisposeAsync_Should_Not_Throw()
    {
        var cut = RenderComponent<FluentCxColorPalette>();
        await cut.Instance.DisposeAsync();
    }
}
