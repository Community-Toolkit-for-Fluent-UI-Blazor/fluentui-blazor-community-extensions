using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialItemLayoutTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var layout = new SleekDialItemLayout();

        Assert.Equal(0, layout.X);
        Assert.Equal(0, layout.Y);
        Assert.Equal(0, layout.Angle);
        Assert.Equal(0, layout.Radius);
        Assert.Equal(TimeSpan.Zero, layout.AnimationDelay);
        Assert.Equal(string.Empty, layout.Transform);
        Assert.Equal(string.Empty, layout.Transition);
        Assert.Equal(string.Empty, layout.FinalTransform);
        Assert.Equal(0, layout.InitialOpacity);
        Assert.Equal(1, layout.FinalOpacity);
        Assert.Equal(string.Empty, layout.CssClass);
    }
}
