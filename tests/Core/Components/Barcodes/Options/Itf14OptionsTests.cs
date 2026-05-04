using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Itf14OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Itf14Options();

        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.False(options.ShowText);
    }
}
