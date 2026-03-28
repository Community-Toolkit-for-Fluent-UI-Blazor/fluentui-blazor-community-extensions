using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class UpcASymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new UpcASymbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.Equal(Symbology.UpcA, symbology.Symbology);
    }
}
