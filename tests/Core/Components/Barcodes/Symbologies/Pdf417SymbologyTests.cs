using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class Pdf417SymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new Pdf417Symbology();

        Assert.Equal(PDF417Mode.Normal, symbology.Mode);
        Assert.Equal(PDF417ErrorCorrectionLevel.Level2, symbology.ErrorLevel);
        Assert.Equal(4, symbology.Columns);
        Assert.Null(symbology.Rows);
        Assert.False(symbology.Compact);
        Assert.False(symbology.AutoMicroGrid);
        Assert.Null(symbology.MicroGrid);
        Assert.Equal(1.0, symbology.ModuleWidth);
        Assert.Equal(2.0, symbology.ModuleHeight);
        Assert.Equal(Symbology.Pdf417, symbology.Symbology);
    }
}
