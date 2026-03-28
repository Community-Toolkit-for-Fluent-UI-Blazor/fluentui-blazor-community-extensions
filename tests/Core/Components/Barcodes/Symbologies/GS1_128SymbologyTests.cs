using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class GS1_128SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new GS1_128Symbology();

        Assert.Equal(Gs1ValidationMode.Strict, symbology.ValidationMode);
        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.True(symbology.ShowText);
        Assert.Equal(Symbology.GS1_128, symbology.Symbology);
    }
}
