using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Code128SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Code128Symbology();

        Assert.Equal(Code128Subset.Auto, symbology.Subset);
        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.True(symbology.ShowText);
        Assert.Equal(Symbology.Code128, symbology.Symbology);
    }
}
