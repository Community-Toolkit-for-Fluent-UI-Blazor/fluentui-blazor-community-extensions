using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.ColorSpace.Spaces;

public class Srgb8ColorsTests
{
    [Fact]
    public void Srgb8Colors_TryGetColor_ReturnsNamedColor()
    {
        var success = Srgb8Colors.TryGetColor("rebeccapurple", out var color);

        Assert.True(success);
        Assert.Equal(new Srgb8(102, 51, 153), color);
    }

    [Fact]
    public void Srgb8Colors_TryGetColor_IsCaseInsensitive()
    {
        var success = Srgb8Colors.TryGetColor("RED", out var color);

        Assert.True(success);
        Assert.Equal(new Srgb8(255, 0, 0), color);
    }

    [Fact]
    public void Srgb8Colors_TryGetColor_ReturnsFalseForUnknown()
    {
        var success = Srgb8Colors.TryGetColor("not-a-color", out _);

        Assert.False(success);
    }
}
