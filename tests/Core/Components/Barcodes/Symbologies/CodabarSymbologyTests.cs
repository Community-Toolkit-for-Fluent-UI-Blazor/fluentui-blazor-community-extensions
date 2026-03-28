using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class CodabarSymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new CodabarSymbology();

        Assert.Equal(1.0, symbology.ModuleWidth);
        Assert.Equal(3.0, symbology.WideRatio);
        Assert.Equal(50.0, symbology.ModuleHeight);
        Assert.True(symbology.ShowText);
        Assert.Equal(Symbology.Codabar, symbology.Symbology);
    }
}
