using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Code93SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Code93Symbology();

        Assert.Equal(1, symbology.ModuleWidth);
        Assert.Equal(50, symbology.ModuleHeight);
        Assert.True(symbology.ShowText);
        Assert.True(symbology.IsExtended);
        Assert.Equal(Symbology.Code93, symbology.Symbology);
    }
}
