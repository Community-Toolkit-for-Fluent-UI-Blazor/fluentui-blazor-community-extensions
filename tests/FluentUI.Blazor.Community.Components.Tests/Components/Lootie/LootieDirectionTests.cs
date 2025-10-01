using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lottie;
public class LottieDirectionTests
{
    [Fact]
    public void Forward_Should_Have_Value_One()
    {
        Assert.Equal(1, (int)LottieDirection.Forward);
    }

    [Fact]
    public void Backward_Should_Have_Value_MinusOne()
    {
        Assert.Equal(-1, (int)LottieDirection.Backward);
    }

    [Theory]
    [InlineData(1, LottieDirection.Forward)]
    [InlineData(-1, LottieDirection.Backward)]
    public void Can_Parse_Int_To_LottieDirection(int value, LottieDirection expected)
    {
        var direction = (LottieDirection)value;
        Assert.Equal(expected, direction);
    }

    [Theory]
    [InlineData("Forward", LottieDirection.Forward)]
    [InlineData("Backward", LottieDirection.Backward)]
    public void Can_Parse_Name_To_LottieDirection(string name, LottieDirection expected)
    {
        var parsed = (LottieDirection)Enum.Parse(typeof(LottieDirection), name);
        Assert.Equal(expected, parsed);
    }
}
