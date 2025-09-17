using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Effects;

public class CssLengthTests
{
    [Fact]
    public void Constructor_ValidValue_DefaultUnit_Pixels()
    {
        var length = new CssLength(10);
        Assert.Equal("10px", length.ToString());
    }

    [Fact]
    public void Constructor_ValidValue_SpecificUnit()
    {
        var length = new CssLength(25.5, LengthUnit.Percent);
        Assert.Equal("25.5%", length.ToString());
    }

    [Fact]
    public void Constructor_NegativeValue_Throws()
    {
        Assert.Throws<ArgumentException>(() => new CssLength(-1));
    }

    [Fact]
    public void Constructor_NaNValue_Throws()
    {
        Assert.Throws<ArgumentException>(() => new CssLength(double.NaN));
    }

    [Fact]
    public void Constructor_InfinityValue_Throws()
    {
        Assert.Throws<ArgumentException>(() => new CssLength(double.PositiveInfinity));
    }

    [Fact]
    public void ToString_FormatsValueWithUnit()
    {
        var length = new CssLength(12.345, LengthUnit.Em);
        Assert.Equal("12.35em", length.ToString());
    }

    [Fact]
    public void ToString_FormatsZeroValue()
    {
        var length = new CssLength(0, LengthUnit.Rem);
        Assert.Equal("0rem", length.ToString());
    }
}
