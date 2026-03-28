using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Symbologies;

public class QRCodeSymbologyTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var symbology = new QRCodeSymbology();

        Assert.Equal(1, symbology.ModuleSize);
        Assert.Equal(QRVersion.Auto, symbology.Version);
        Assert.Equal(default, symbology.ErrorCorrection);
        Assert.Equal(default, symbology.EncodingMode);
        Assert.Equal(Symbology.QRCode, symbology.Symbology);
    }
}
