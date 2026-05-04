using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Code128OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Code128Options();

        Assert.Equal(Code128Subset.Auto, options.Subset);
        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(50, options.ModuleHeight);
        Assert.True(options.ShowText);
    }
}
