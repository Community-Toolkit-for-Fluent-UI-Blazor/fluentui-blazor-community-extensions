using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes;

public class BarcodeRectangleTests
{
    [Fact]
    public void Constructor_SetsValues()
    {
        var rectangle = new BarcodeRectangle(1, 2, 3, 4);

        Assert.Equal(1, rectangle.X);
        Assert.Equal(2, rectangle.Y);
        Assert.Equal(3, rectangle.Width);
        Assert.Equal(4, rectangle.Height);
    }

    [Fact]
    public void Properties_CanBeUpdated()
    {
        var rectangle = new BarcodeRectangle(0, 0, 1, 1)
        {
            X = 5,
            Y = 6,
            Width = 7,
            Height = 8
        };

        Assert.Equal(5, rectangle.X);
        Assert.Equal(6, rectangle.Y);
        Assert.Equal(7, rectangle.Width);
        Assert.Equal(8, rectangle.Height);
    }
}
