using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Code39OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Code39Options();

        Assert.Equal(3.0, options.WideRatio);
        Assert.False(options.EnableChecksum);
        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.False(options.IsExtended);
    }
}
