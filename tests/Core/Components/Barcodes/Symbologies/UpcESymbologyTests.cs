using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class UpcESymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new UpcESymbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.Equal(0, symbology.NumberSystem);
        Assert.Equal(Symbology.UpcE, symbology.Symbology);
    }
}
