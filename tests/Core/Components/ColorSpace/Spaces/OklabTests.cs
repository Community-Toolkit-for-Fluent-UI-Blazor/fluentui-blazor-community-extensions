using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class OklabTests
{
    [Fact]
    public void Oklab_EqualityAndHashCode()
    {
        var left = new Oklab(0.1, 0.2, 0.3);
        var right = new Oklab(0.1, 0.2, 0.3);
        var other = new Oklab(0.2, 0.2, 0.3);

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
