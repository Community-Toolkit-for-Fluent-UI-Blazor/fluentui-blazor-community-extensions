using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes.Options;

public class Pdf417OptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new Pdf417Options();

        Assert.Equal(PDF417Mode.Normal, options.Mode);
        Assert.Equal(PDF417ErrorCorrectionLevel.Level2, options.ErrorLevel);
        Assert.Equal(4, options.Columns);
        Assert.Null(options.Rows);
        Assert.False(options.Compact);
        Assert.False(options.AutoMicroGrid);
        Assert.Null(options.MicroGrid);
        Assert.Equal(1, options.ModuleWidth);
        Assert.Equal(2, options.ModuleHeight);
    }
}
