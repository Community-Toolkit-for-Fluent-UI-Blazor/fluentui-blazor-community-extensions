using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Ean8OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Ean8Options();

        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.Equal(55, options.GuardModuleHeight);
    }
}
