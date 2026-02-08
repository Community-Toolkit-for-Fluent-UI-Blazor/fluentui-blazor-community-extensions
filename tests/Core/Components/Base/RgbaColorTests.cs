using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Base;

public class RgbaColorTests
{
    public static TheoryData<double> InvalidAlphaValues =>
        new() { -0.1, 1.1, double.NaN, double.PositiveInfinity, double.NegativeInfinity };

    [Theory]
    [MemberData(nameof(InvalidAlphaValues))]
    public void Constructor_InvalidAlpha_Throws(double alpha)
    {
        var exception = Assert.Throws<ArgumentException>(() => new RgbaColor(1, 2, 3, alpha));

        Assert.Equal("a", exception.ParamName);
    }

    [Fact]
    public void ToString_FormatsCss()
    {
        var color = new RgbaColor(10, 20, 30, 0.5);

        Assert.Equal("rgba(10,20,30,0.5)", color.ToString());
    }
}
