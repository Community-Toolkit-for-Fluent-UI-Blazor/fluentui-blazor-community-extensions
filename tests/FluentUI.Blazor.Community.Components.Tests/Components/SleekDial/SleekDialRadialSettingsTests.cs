using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialRadialSettingsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var settings = new SleekDialRadialSettings();

        Assert.Equal(-1, settings.StartAngle);
        Assert.Equal(-1, settings.EndAngle);
        Assert.Equal("110px", settings.Offset);
        Assert.Equal(SleekDialRadialDirection.Clockwise, settings.Direction);
    }

    [Fact]
    public void Can_Set_And_Get_StartAngle()
    {
        var settings = new SleekDialRadialSettings { StartAngle = 45 };
        Assert.Equal(45, settings.StartAngle);
    }

    [Fact]
    public void Can_Set_And_Get_EndAngle()
    {
        var settings = new SleekDialRadialSettings { EndAngle = 270 };
        Assert.Equal(270, settings.EndAngle);
    }

    [Fact]
    public void Can_Set_And_Get_Offset()
    {
        var settings = new SleekDialRadialSettings { Offset = "150px" };
        Assert.Equal("150px", settings.Offset);
    }

    [Fact]
    public void Can_Set_And_Get_Direction()
    {
        var settings = new SleekDialRadialSettings { Direction = SleekDialRadialDirection.Counterclockwise };
        Assert.Equal(SleekDialRadialDirection.Counterclockwise, settings.Direction);
    }
}
