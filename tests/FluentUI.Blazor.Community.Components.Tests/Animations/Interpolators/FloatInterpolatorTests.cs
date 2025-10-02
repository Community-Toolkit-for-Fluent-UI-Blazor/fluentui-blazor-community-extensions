using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations.Interpolators;
public class FloatInterpolatorTests
{
    [Theory]
    [InlineData(0f, 10f, 0.0, 0f)]
    [InlineData(0f, 10f, 1.0, 10f)]
    [InlineData(0f, 10f, 0.5, 5f)]
    [InlineData(-5f, 5f, 0.5, 0f)]
    public void Lerp_ReturnsExpectedValue(float start, float end, double amount, float expected)
    {
        var interpolator = new FloatInterpolator();
        var result = interpolator.Lerp(start, end, amount);
        Assert.Equal(expected, result, 6);
    }
}
