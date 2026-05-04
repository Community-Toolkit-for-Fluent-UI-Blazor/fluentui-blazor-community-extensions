using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class BarcodeQuietZoneOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new BarcodeQuietZoneOptions();

        Assert.True(options.Enabled);
        Assert.Equal(4, options.Padding.Left);
        Assert.Equal(4, options.Padding.Top);
        Assert.Equal(4, options.Padding.Right);
        Assert.Equal(4, options.Padding.Bottom);
    }
}
