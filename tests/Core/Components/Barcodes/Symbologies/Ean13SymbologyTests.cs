using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Ean13SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Ean13Symbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.Equal(Symbology.Ean13, symbology.Symbology);
    }
}
