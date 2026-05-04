using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class XyyTests
{
    [Fact]
    public void Xyy_EqualityAndHashCode()
    {
        var left = new Xyy(0.1, 0.2, 0.3);
        var right = new Xyy(0.1, 0.2, 0.3);
        var other = new Xyy(0.1, 0.25, 0.3);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Contains("x:", left.ToString());
    }

    [Fact]
    public void Xyy_ToXyy_HandlesZeroSum()
    {
        var xyz = new Xyz(0, 0, 0);

        var xyy = Xyy.ToXyy(xyz);

        Assert.Equal(0, xyy.X, 12);
        Assert.Equal(0, xyy.Y, 12);
        Assert.Equal(0, xyy.Y2, 12);
    }

    [Fact]
    public void Xyy_ToXyy_ConvertsValues()
    {
        var xyz = new Xyz(0.5, 1.0, 1.5);

        var xyy = Xyy.ToXyy(xyz);

        Assert.Equal(0.16666666666666666, xyy.X, 12);
        Assert.Equal(0.3333333333333333, xyy.Y, 12);
        Assert.Equal(1.0, xyy.Y2, 12);
    }
}
