using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;
using SkiaSharp;

namespace FluentUI.Blazor.Community.Components.Tests.Helpers;

public class ImagePaletteTests
{
    [Fact]
    public void ExtractColorsFromImage_ReturnsDominantColor_ForSolidImage()
    {
        // Arrange : image 10x10, full red
        using var bmp = new SKBitmap(10, 10);
        using (var canvas = new SKCanvas(bmp))
        {
            canvas.Clear(SKColors.Red);
        }
        using var ms = new MemoryStream();
        using (var image = SKImage.FromBitmap(bmp))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
            data.SaveTo(ms);
        }
        ms.Position = 0;

        // Act
        var result = ImagePalette.ExtractColorsFromImage(ms, 1);

        // Assert
        Assert.Single(result);
        Assert.Equal("#FF0000", result[0]);
    }

    [Fact]
    public void ExtractColorsFromImage_ReturnsMultipleColors_ForMultiColorImage()
    {
        // Arrange : image 2x1, half red, half green
        using var bmp = new SKBitmap(2, 1);
        bmp.SetPixel(0, 0, SKColors.Red);
        bmp.SetPixel(1, 0, SKColors.Green);
        using var ms = new MemoryStream();
        using (var image = SKImage.FromBitmap(bmp))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
            data.SaveTo(ms);
        }

        ms.Position = 0;

        // Act
        var result = ImagePalette.ExtractColorsFromImage(ms, 2);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("#FF0000", result);
        Assert.Contains("#008000", result);
    }

    [Fact]
    public void ExtractColorsFromImage_ReturnsEmptyList_ForEmptyStream()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act & Assert
        var result = ImagePalette.ExtractColorsFromImage(ms, 1);
        Assert.Empty(result);
    }

    [Fact]
    public void ExtractColorsFromImage_ThrowsNull_ForNullStream()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ImagePalette.ExtractColorsFromImage(null, 1));
    }

    [Fact]
    public void ExtractColorsFromImage_ThrowsOutOfRangeException_ForCountBelowOne()
    {
        // Arrange
        using var ms = new MemoryStream();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ImagePalette.ExtractColorsFromImage(ms, 0));
    }

    [Fact]
    public void ExtractColorsFromImage_ReturnsLessColors_IfCountExceedsPalette()
    {
        // Arrange : image 1x1, blue
        using var bmp = new SKBitmap(1, 1);
        bmp.SetPixel(0, 0, SKColors.Blue);
        using var ms = new MemoryStream();
        using (var image = SKImage.FromBitmap(bmp))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
            data.SaveTo(ms);
        }
        ms.Position = 0;

        // Act
        var result = ImagePalette.ExtractColorsFromImage(ms, 5);

        // Assert
        Assert.Single(result);
        Assert.Equal("#0000FF", result[0]);
    }
}

