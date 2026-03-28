using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Ean8SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Ean8Symbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.Equal(Symbology.Ean8, symbology.Symbology);
    }
}
