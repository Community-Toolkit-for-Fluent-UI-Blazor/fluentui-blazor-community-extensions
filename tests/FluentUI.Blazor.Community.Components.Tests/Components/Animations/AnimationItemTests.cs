using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Animations;
public class AnimationItemTests : TestBase
{
    [Fact]
    public void AnimationItem_Initializes_Id()
    {
        // Arrange & Act
        var item = new AnimationItem();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(item.AnimatedElement.Id));
    }

    [Fact]
    public void AnimationItem_Parameters_AreSetCorrectly()
    {
        // Arrange
        var cut = RenderComponent<AnimationItem>(parameters => parameters
            .Add(p => p.BackgroundColor, "#fff")
            .Add(p => p.Color, "#000")
            .Add(p => p.OffsetX, 10)
            .Add(p => p.OffsetY, 20)
            .Add(p => p.Opacity, 0.5)
            .Add(p => p.Rotation, 45)
            .Add(p => p.ScaleX, 2)
            .Add(p => p.ScaleY, 3)
            .Add(p => p.Value, 99)
            .Add(p => p.Width, 100)
            .Add(p => p.Height, 200)
            .Add(p => p.Left, "10px")
            .Add(p => p.Top, "20px")
            .Add(p => p.Right, "30px")
            .Add(p => p.Bottom, "40px")
            .Add(p => p.ZIndex, 5)
        );

        var instance = cut.Instance;

        // Assert
        Assert.Equal("#fff", instance.BackgroundColor);
        Assert.Equal("#000", instance.Color);
        Assert.Equal(10, instance.OffsetX);
        Assert.Equal(20, instance.OffsetY);
        Assert.Equal(0.5, instance.Opacity);
        Assert.Equal(45, instance.Rotation);
        Assert.Equal(2, instance.ScaleX);
        Assert.Equal(3, instance.ScaleY);
        Assert.Equal(99, instance.Value);
        Assert.Equal(100, instance.Width);
        Assert.Equal(200, instance.Height);
        Assert.Equal("10px", instance.Left);
        Assert.Equal("20px", instance.Top);
        Assert.Equal("30px", instance.Right);
        Assert.Equal("40px", instance.Bottom);
        Assert.Equal(5, instance.ZIndex);
    }

    [Fact]
    public void AnimationItem_InternalCss_ContainsAnimationItemClass()
    {
        // Arrange
        var cut = RenderComponent<AnimationItem>();

        // Assert
        Assert.Contains("animation-item", cut.Markup);
    }

    [Fact]
    public void AnimationItem_InternalStyle_BuildsCorrectly()
    {
        // Arrange
        var cut = RenderComponent<AnimationItem>(parameters => parameters
            .Add(p => p.Width, 100)
            .Add(p => p.Height, 200)
            .Add(p => p.Left, "10px")
            .Add(p => p.Top, "20px")
            .Add(p => p.Right, "30px")
            .Add(p => p.Bottom, "40px")
            .Add(p => p.ZIndex, 5)
        );

        // Assert
        Assert.Contains("--animation-item-width: 100px", cut.Markup);
        Assert.Contains("--animation-item-height: 200px", cut.Markup);
        Assert.Contains("--animation-item-left: 10px", cut.Markup);
        Assert.Contains("--animation-item-top: 20px", cut.Markup);
        Assert.Contains("--animation-item-right: 30px", cut.Markup);
        Assert.Contains("--animation-item-bottom: 40px", cut.Markup);
        Assert.Contains("--animation-item-z-index: 5", cut.Markup);
    }

    [Fact]
    public void AnimationItem_AnimatedElement_ReflectsParameters()
    {
        // Arrange
        var cut = RenderComponent<AnimationItem>(parameters => parameters
            .Add(p => p.BackgroundColor, "#fff")
            .Add(p => p.Color, "#000")
            .Add(p => p.OffsetX, 10)
            .Add(p => p.OffsetY, 20)
            .Add(p => p.Opacity, 0.5)
            .Add(p => p.Rotation, 45)
            .Add(p => p.ScaleX, 2)
            .Add(p => p.ScaleY, 3)
            .Add(p => p.Value, 99)
        );

        var element = cut.Instance.AnimatedElement;

        // Assert
        Assert.Equal("#fff", element.BackgroundColor);
        Assert.Equal("#000", element.Color);
        Assert.Equal(10, element.OffsetX);
        Assert.Equal(20, element.OffsetY);
        Assert.Equal(0.5, element.Opacity);
        Assert.Equal(45, element.Rotation);
        Assert.Equal(2, element.ScaleX);
        Assert.Equal(3, element.ScaleY);
        Assert.Equal(99, element.Value);
    }
}
