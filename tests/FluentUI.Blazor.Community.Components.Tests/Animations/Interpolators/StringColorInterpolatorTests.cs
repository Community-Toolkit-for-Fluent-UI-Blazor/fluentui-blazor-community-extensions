using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations.Interpolators;
public class StringColorInterpolatorTests
{
    [Theory]
    [InlineData("#000000", "#FFFFFF", 0.0, "#000000")]
    [InlineData("#000000", "#FFFFFF", 1.0, "#FFFFFF")]
    [InlineData("#000000", "#FFFFFF", 0.5, "#808080")]
    [InlineData("#FF0000", "#00FF00", 0.5, "#808000")]
    [InlineData("red", "blue", 0.5, "#800080")]
    public void Lerp_ReturnsExpectedHtmlColor(string start, string end, double amount, string expected)
    {
        var interpolator = new StringColorInterpolator();
        var result = interpolator.Lerp(start, end, amount);
        Assert.Equal(expected, result, ignoreCase: true);
    }
}
