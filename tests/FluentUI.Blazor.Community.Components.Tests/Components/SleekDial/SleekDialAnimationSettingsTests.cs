using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialAnimationSettingsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var settings = new SleekDialAnimationSettings();

        Assert.Equal(TimeSpan.FromMilliseconds(400), settings.Duration);
        Assert.Equal(TimeSpan.Zero, settings.Delay);
        Assert.Equal(SleekDialAnimation.Fade, settings.Animation);
    }

    [Fact]
    public void Can_Set_And_Get_Duration()
    {
        var settings = new SleekDialAnimationSettings();
        var expected = TimeSpan.FromSeconds(2);

        settings.Duration = expected;

        Assert.Equal(expected, settings.Duration);
    }

    [Fact]
    public void Can_Set_And_Get_Delay()
    {
        var settings = new SleekDialAnimationSettings();
        var expected = TimeSpan.FromMilliseconds(150);

        settings.Delay = expected;

        Assert.Equal(expected, settings.Delay);
    }

    [Fact]
    public void Can_Set_And_Get_Animation()
    {
        var settings = new SleekDialAnimationSettings();
        var expected = SleekDialAnimation.Fade;

        settings.Animation = expected;

        Assert.Equal(expected, settings.Animation);
    }
}
