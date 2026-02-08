using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureLoadingBehaviorTests
{
    [Theory]
    [InlineData(PictureLoadingBehavior.Auto, 0)]
    [InlineData(PictureLoadingBehavior.Lazy, 1)]
    [InlineData(PictureLoadingBehavior.Eager, 2)]
    public void PictureLoadingBehavior_HasExpectedValue(PictureLoadingBehavior value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
