using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Vision;

public class ConfusionLineTests
{
    [Fact]
    public void ConfusionLine_EqualityAndHashCode()
    {
        var left = ConfusionLineSet.Protanopia;
        var right = ConfusionLineSet.Protanopia;
        var other = ConfusionLineSet.Deutanopia;

        Assert.True(left == right);
        Assert.False(left != right);
        Assert.True(left.Equals(right));
        Assert.False(left.Equals(other));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Contains("Slope", left.ToString());
    }
}
