using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class BarcodeRenderingOptionsTests
{
    [Fact]
    public void Defaults_AreInstantiated()
    {
        var options = new BarcodeRenderingOptions();

        Assert.NotNull(options.Background);
        Assert.NotNull(options.Foreground);
        Assert.NotNull(options.View);
        Assert.NotNull(options.QuietZone);
        Assert.NotNull(options.Label);
    }
}
