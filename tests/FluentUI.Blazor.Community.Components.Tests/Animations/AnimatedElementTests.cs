using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class AnimatedElementTests
{
    private class DummyInterpolator : IInterpolator<double>
    {
        public double Lerp(double start, double end, double amount) => start + (end - start) * amount;
    }

    [Fact]
    public void Update_UpdatesProperties_WhenStatesAreSet()
    {
        var now = DateTime.Now;
        var state = new AnimationState<double>
        {
            StartValue = 0,
            EndValue = 100,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = now
        };
        var element = new AnimatedElement
        {
            OffsetXState = state,
            OffsetYState = state,
            ScaleXState = state,
            ScaleYState = state,
            RotationState = state,
            OpacityState = state,
            ValueState = state
        };
        element.Update(now + TimeSpan.FromSeconds(1));
        Assert.Equal(100, element.OffsetX);
        Assert.Equal(100, element.OffsetY);
        Assert.Equal(100, element.ScaleX);
        Assert.Equal(100, element.ScaleY);
        Assert.Equal(100, element.Rotation);
        Assert.Equal(100, element.Opacity);
        Assert.Equal(100, element.Value);
    }

    [Fact]
    public void GetDiff_ReturnsDifferences()
    {
        var prev = new AnimatedElement
        {
            OffsetX = 1,
            OffsetY = 2,
            ScaleX = 1,
            ScaleY = 1,
            Rotation = 0,
            Color = "red",
            BackgroundColor = "blue",
            Opacity = 1,
            Value = 0
        };
        var curr = prev.Clone();
        curr.OffsetX = 5;
        curr.Color = "green";
        curr.Opacity = 0.5;
        var diff = curr.GetDiff(prev);
        Assert.Equal(3, diff.Count);
        Assert.Equal(5d, diff["offsetX"]);
        Assert.Equal("green", diff["color"]);
        Assert.Equal(0.5d, diff["opacity"]);
    }

    [Fact]
    public void Clone_CopiesAllProperties()
    {
        var element = new AnimatedElement
        {
            Id = "test",
            OffsetX = 1,
            OffsetY = 2,
            ScaleX = 3,
            ScaleY = 4,
            Rotation = 5,
            Color = "red",
            BackgroundColor = "blue",
            Opacity = 0.5,
            Value = 42
        };
        var clone = element.Clone();
        Assert.Equal(element.Id, clone.Id);
        Assert.Equal(element.OffsetX, clone.OffsetX);
        Assert.Equal(element.OffsetY, clone.OffsetY);
        Assert.Equal(element.ScaleX, clone.ScaleX);
        Assert.Equal(element.ScaleY, clone.ScaleY);
        Assert.Equal(element.Rotation, clone.Rotation);
        Assert.Equal(element.Color, clone.Color);
        Assert.Equal(element.BackgroundColor, clone.BackgroundColor);
        Assert.Equal(element.Opacity, clone.Opacity);
        Assert.Equal(element.Value, clone.Value);
    }
}
