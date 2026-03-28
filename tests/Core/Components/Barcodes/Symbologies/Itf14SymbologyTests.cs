using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Itf14SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Itf14Symbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.True(symbology.ShowText);
        Assert.Equal(Symbology.Itf14, symbology.Symbology);
    }
}
