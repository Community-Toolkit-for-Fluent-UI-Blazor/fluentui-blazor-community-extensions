using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Renderers;

public class BarcodeRendererFactoryTests
{
    [Fact]
    public void Get_CreatesRendererForSymbology()
    {
        var factory = new BarcodeRendererFactory();
        var symbology = new Code128Symbology();
        SetProperty(symbology, nameof(Code128Symbology.ModuleWidth), 2);
        SetProperty(symbology, nameof(Code128Symbology.ModuleHeight), 7);
        SetProperty(symbology, nameof(Code128Symbology.ShowText), false);
        SetProperty(symbology, nameof(Code128Symbology.Subset), Code128Subset.B);

        var renderer = factory.Get(symbology, () => "1234");

        var typed = Assert.IsType<Barcode1DComposer<Code128Options>>(renderer);
        Assert.Equal(2, typed.Options.ModuleWidth);
        Assert.Equal(7, typed.Options.ModuleHeight);
        Assert.False(typed.Options.ShowText);
        Assert.Equal(Code128Subset.B, typed.Options.Subset);
    }

    [Fact]
    public void Get_UpdatesRendererOptionsWhenSymbologyChanges()
    {
        var factory = new BarcodeRendererFactory();
        var symbology = new Code128Symbology();
        SetProperty(symbology, nameof(Code128Symbology.ModuleWidth), 1);
        SetProperty(symbology, nameof(Code128Symbology.ModuleHeight), 5);
        SetProperty(symbology, nameof(Code128Symbology.ShowText), true);

        var renderer1 = factory.Get(symbology, () => "1234");
        SetProperty(symbology, nameof(Code128Symbology.ModuleWidth), 4);
        SetProperty(symbology, nameof(Code128Symbology.ModuleHeight), 9);
        SetProperty(symbology, nameof(Code128Symbology.ShowText), false);
        SetProperty(symbology, nameof(Code128Symbology.Subset), Code128Subset.C);

        var renderer2 = factory.Get(symbology, () => "1234");

        Assert.Same(renderer1, renderer2);
        var typed = Assert.IsType<Barcode1DComposer<Code128Options>>(renderer2);
        Assert.Equal(4, typed.Options.ModuleWidth);
        Assert.Equal(9, typed.Options.ModuleHeight);
        Assert.False(typed.Options.ShowText);
        Assert.Equal(Code128Subset.C, typed.Options.Subset);
    }

    [Fact]
    public void Get_UnknownSymbology_ReturnsNull()
    {
        var factory = new BarcodeRendererFactory();
        var symbology = new FakeSymbology();

        var renderer = factory.Get(symbology, () => "DATA");

        Assert.Null(renderer);
    }

    private sealed class FakeSymbology : ISymbology
    {
        public Symbology Symbology => (Symbology)999;
    }

    private static void SetProperty<T>(object target, string name, T value)
    {
        target.GetType().GetProperty(name)?.SetValue(target, value);
    }
}
