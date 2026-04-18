using FluentUI.Blazor.Community.Components.Charts.Helpers;
using Xunit;

namespace Components.Tests.Components.Charts.Helpers;

public class NumericTickGeneratorTests
{
    [Fact]
    public void Generate_ReturnsEmptyForInvalidRange()
    {
        var ticks = NumericTickGenerator.Generate(double.NaN, 10);

        Assert.Empty(ticks);
    }

    [Fact]
    public void Generate_SwapsMinMaxWhenReversed()
    {
        var ticks = NumericTickGenerator.Generate(10, 0, 3);

        Assert.NotEmpty(ticks);
        Assert.True(ticks.First() <= ticks.Last());
    }

    [Fact]
    public void Generate_ExpandsDegenerateRange()
    {
        var ticks = NumericTickGenerator.Generate(5, 5, 3);

        Assert.NotEmpty(ticks);
        Assert.Contains(5, ticks);
    }
}
