using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Code39SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Code39Symbology();

        Assert.Equal(3.0, symbology.WideRatio);
        Assert.False(symbology.EnableChecksum);
        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.True(symbology.IsExtended);
        Assert.Equal(Symbology.Code39, symbology.Symbology);
    }
}
