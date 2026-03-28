using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes;

public class Barcode1DBarTests
{
    [Fact]
    public void Properties_SetInternalValues()
    {
        var bar = new Barcode1DBar
        {
            X = 1,
            Y = 2,
            Width = 3,
            Height = 4
        };

        Assert.Equal(1, bar.X);
        Assert.Equal(2, bar.Y);
        Assert.Equal(3, bar.Width);
        Assert.Equal(4, bar.Height);
    }
}
