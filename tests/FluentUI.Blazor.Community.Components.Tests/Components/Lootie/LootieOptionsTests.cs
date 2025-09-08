using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Lottie;

public class LottieOptionsTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        // Arrange
        var path = "animation.json";
        var loop = false;
        var autoplay = false;
        var speed = 2.5;
        var renderer = LottieRenderer.Canvas;

        // Act
        var options = new LottieOptions(path, loop, autoplay, speed, renderer);

        // Assert
        Assert.Equal(path, options.Path);
        Assert.Equal(loop, options.Loop);
        Assert.Equal(autoplay, options.Autoplay);
        Assert.Equal(speed, options.Speed);
        Assert.Equal(renderer, options.Renderer);
    }

    [Fact]
    public void Constructor_UsesDefaultValues()
    {
        // Arrange
        var path = "default.json";

        // Act
        var options = new LottieOptions(path);

        // Assert
        Assert.Equal(path, options.Path);
        Assert.True(options.Loop);
        Assert.True(options.Autoplay);
        Assert.Equal(1, options.Speed);
        Assert.Equal(LottieRenderer.Svg, options.Renderer);
    }

    [Fact]
    public void Records_AreEqual_WhenPropertiesAreEqual()
    {
        // Arrange
        var a = new LottieOptions("a.json", true, false, 1.2, LottieRenderer.Html);
        var b = new LottieOptions("a.json", true, false, 1.2, LottieRenderer.Html);

        // Assert
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void With_CreatesModifiedCopy()
    {
        // Arrange
        var original = new LottieOptions("a.json");
        var modified = original with { Speed = 3.0, Renderer = LottieRenderer.Canvas };

        // Assert
        Assert.Equal("a.json", modified.Path);
        Assert.Equal(3.0, modified.Speed);
        Assert.Equal(LottieRenderer.Canvas, modified.Renderer);
        Assert.NotEqual(original, modified);
    }
}
