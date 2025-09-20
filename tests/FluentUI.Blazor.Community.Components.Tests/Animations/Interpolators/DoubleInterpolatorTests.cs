using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations.Interpolators;
public class DoubleInterpolatorTests
{
    [Theory]
    [InlineData(0.0, 10.0, 0.0, 0.0)]
    [InlineData(0.0, 10.0, 1.0, 10.0)]
    [InlineData(0.0, 10.0, 0.5, 5.0)]
    [InlineData(-5.0, 5.0, 0.5, 0.0)]
    public void Lerp_ReturnsExpectedValue(double start, double end, double amount, double expected)
    {
        var interpolator = new DoubleInterpolator();
        var result = interpolator.Lerp(start, end, amount);
        Assert.Equal(expected, result, 6);
    }
}




