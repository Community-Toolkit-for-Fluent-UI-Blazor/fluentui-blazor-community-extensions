using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;
public class PaletteModeTests
{
    [Theory]
    [InlineData(ColorPaletteMode.None)]
    [InlineData(ColorPaletteMode.Provided)]
    [InlineData(ColorPaletteMode.Random)]
    [InlineData(ColorPaletteMode.Gradient)]
    [InlineData(ColorPaletteMode.CustomGradient)]
    [InlineData(ColorPaletteMode.Complementary)]
    [InlineData(ColorPaletteMode.Analogous)]
    [InlineData(ColorPaletteMode.Triadic)]
    [InlineData(ColorPaletteMode.Tetradic)]
    [InlineData(ColorPaletteMode.SplitComplementary)]
    [InlineData(ColorPaletteMode.Monochrome)]
    [InlineData(ColorPaletteMode.Warm)]
    [InlineData(ColorPaletteMode.Cool)]
    [InlineData(ColorPaletteMode.Pastel)]
    [InlineData(ColorPaletteMode.Neon)]
    [InlineData(ColorPaletteMode.Greyscale)]
    [InlineData(ColorPaletteMode.AccessibilitySafe)]
    [InlineData(ColorPaletteMode.FromImage)]
    public void PaletteMode_Enum_ShouldBeDefined(ColorPaletteMode mode)
    {
        Assert.True(Enum.IsDefined(mode));
    }
}
