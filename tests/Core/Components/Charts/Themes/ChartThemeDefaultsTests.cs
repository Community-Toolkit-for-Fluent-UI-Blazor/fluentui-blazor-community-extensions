using FluentUI.Blazor.Community.Components.Charts.Themes;
using Xunit;

namespace Components.Tests.Components.Charts.Themes;

public class ChartThemeDefaultsTests
{
    [Fact]
    public void ChartThemeContext_Defaults_AreInitialized()
    {
        var context = new ChartThemeContext();

        Assert.NotNull(context.Theme);
        Assert.NotNull(context.Density);
        Assert.NotNull(context.Contrast);
        Assert.NotNull(context.ComputedValues);
    }

    [Fact]
    public void ChartPalette_Defaults_AreInitialized()
    {
        var palette = new ChartPalette();

        Assert.NotNull(palette.Series);
        Assert.NotNull(palette.StrokeSeries);
    }
}
