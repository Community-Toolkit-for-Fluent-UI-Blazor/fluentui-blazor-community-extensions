using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialAnimationSettingsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var settings = new SleekDialAnimationSettings();

        Assert.Equal(SleekDialAnimationType.Fade, settings.Animation);
        Assert.Equal(SleekDialEasing.EaseOut, settings.Easing);
        Assert.Equal(TimeSpan.FromMilliseconds(400), settings.Duration);
        Assert.Equal(TimeSpan.Zero, settings.Delay);
        Assert.True(settings.Stagger);
    }
}
