using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Code93OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Code93Options();

        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.False(options.IsExtended);
        Assert.False(options.ShowText);
    }
}
