using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionVariantTests
{
    [Fact]
    public void SetAndTryGet_ReturnsValue()
    {
        var variant = new MotionVariant().Set("x", 2.0);

        var found = variant.TryGet<double>("x", out var value);

        Assert.True(found);
        Assert.Equal(2.0, value, 6);
    }

    [Fact]
    public void TryGet_ReturnsFalseForMissingKey()
    {
        var variant = new MotionVariant();

        var found = variant.TryGet<int>("missing", out var value);

        Assert.False(found);
        Assert.Equal(0, value);
    }
}
