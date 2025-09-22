using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ColorPalette;
public class GradientStrategyTests
{
    [Theory]
    [InlineData(GradientStrategy.Shades)]
    [InlineData(GradientStrategy.Tints)]
    [InlineData(GradientStrategy.Saturation)]
    [InlineData(GradientStrategy.HueShift)]
    public void GradientStrategy_Enum_ShouldBeDefined(GradientStrategy strategy)
    {
        Assert.True(Enum.IsDefined(typeof(GradientStrategy), strategy));
    }
}
