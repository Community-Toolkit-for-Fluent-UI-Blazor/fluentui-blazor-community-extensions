using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class CodabarOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new CodabarOptions();

        Assert.Equal(1.0, options.ModuleWidth);
        Assert.Equal(3.0, options.WideRatio);
        Assert.Equal(50.0, options.ModuleHeight);
        Assert.True(options.ShowText);
    }
}
