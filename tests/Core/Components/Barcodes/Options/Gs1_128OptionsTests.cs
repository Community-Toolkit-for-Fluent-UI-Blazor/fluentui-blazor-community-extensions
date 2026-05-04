using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Gs1_128OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Gs1_128Options();

        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.True(options.ShowText);
    }
}
