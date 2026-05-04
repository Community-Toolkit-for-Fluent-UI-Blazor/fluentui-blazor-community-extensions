using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class UpcAOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new UpcAOptions();

        Assert.Equal(2.0, options.ModuleWidth);
        Assert.Equal(50.0, options.ModuleHeight);
        Assert.Equal(55.0, options.GuardModuleHeight);
    }
}
