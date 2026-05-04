using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers;

public class GS1PatternTests
{
    [Fact]
    public void Add_BuildsBarsForRuns()
    {
        var bars = new List<Barcode1DBar>();
        var x = 0.0;

        GS1Pattern.Add(bars, ref x, 2, 5, "110011");

        Assert.Equal(2, bars.Count);
        Assert.Equal(0, bars[0].X);
        Assert.Equal(4, bars[0].Width);
        Assert.Equal(8, bars[1].X);
        Assert.Equal(4, bars[1].Width);
        Assert.Equal(12, x);
    }
}
