using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartPointTests
{
    [Fact]
    public void ChartPoint_EqualityAndHashCode()
    {
        var left = new ChartPoint(1, 2);
        var right = new ChartPoint(1, 2);
        var other = new ChartPoint(2, 3);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
