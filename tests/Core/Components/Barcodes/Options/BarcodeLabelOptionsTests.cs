using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class BarcodeLabelOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new BarcodeLabelOptions();

        Assert.True(options.Enabled);
        Assert.Equal(12, options.FontSize);
        Assert.Equal(12, options.OffsetY);
        Assert.False(string.IsNullOrWhiteSpace(options.FontFamily));
        Assert.False(string.IsNullOrWhiteSpace(options.Color));
    }
}
