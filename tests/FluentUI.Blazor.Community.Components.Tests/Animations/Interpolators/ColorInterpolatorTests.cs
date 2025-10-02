using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations.Interpolators;
public class ColorInterpolatorTests
{
    [Theory]
    [InlineData(0, 0, 0, 255, 255, 255, 255, 0, 0.0, 0, 0, 0, 255)]
    [InlineData(0, 0, 0, 255, 255, 255, 255, 0, 1.0, 255, 255, 255, 0)]
    [InlineData(0, 0, 0, 255, 255, 255, 255, 0, 0.5, 127, 127, 127, 127)]
    public void Lerp_ReturnsExpectedColor(
       int r1, int g1, int b1, int a1,
       int r2, int g2, int b2, int a2,
       double amount,
       int expectedR,
       int expectedG,
       int expectedB,
       int expectedA)
    {
        var start = Color.FromArgb(a1, r1, g1, b1);
        var end = Color.FromArgb(a2, r2, g2, b2);
        var interpolator = new ColorInterpolator();

        var result = interpolator.Lerp(start, end, amount);

        Assert.Equal(expectedR, result.R);
        Assert.Equal(expectedG, result.G);
        Assert.Equal(expectedB, result.B);
        Assert.Equal(expectedA, result.A);
    }
}
