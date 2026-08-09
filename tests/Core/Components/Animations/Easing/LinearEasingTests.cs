using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Easing;

public class LinearEasingTests
{
    [Fact]
    public void Ease_ReturnsInput()
    {
        var result = LinearEasing.Ease(0.25);

        Assert.Equal(0.25, result, 6);
    }
}
