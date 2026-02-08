using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

public class CssLengthTests
{
    public static TheoryData<double> InvalidValues =>
        new() { double.NaN, double.PositiveInfinity, double.NegativeInfinity, -1 };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public void Constructor_InvalidValue_Throws(double value)
    {
        var exception = Assert.Throws<ArgumentException>(() => new CssLength(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Theory]
    [InlineData(0, LengthUnit.Pixels, "0px")]
    [InlineData(12.5, LengthUnit.Percent, "12.5%")]
    [InlineData(8, LengthUnit.Em, "8em")]
    public void ToString_FormatsCss(double value, LengthUnit unit, string expected)
    {
        var length = new CssLength(value, unit);

        Assert.Equal(expected, length.ToString());
    }
}
