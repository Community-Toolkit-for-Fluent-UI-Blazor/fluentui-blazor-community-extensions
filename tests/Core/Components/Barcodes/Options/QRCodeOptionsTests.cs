using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class QRCodeOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new QRCodeOptions();

        Assert.Equal(1, options.ModuleSize);
        Assert.Equal(QRVersion.Auto, options.Version);
        Assert.Equal(default, options.ErrorCorrection);
        Assert.Equal(default, options.EncodingMode);
    }
}
