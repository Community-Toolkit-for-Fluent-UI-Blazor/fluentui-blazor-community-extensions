using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialRadialSettingsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var settings = new SleekDialRadialSettings();

        Assert.Equal(-1, settings.StartAngle);
        Assert.Equal(-1, settings.EndAngle);
        Assert.Equal("110px", settings.Offset);
        Assert.Equal(SleekDialRadialDirection.Clockwise, settings.Direction);
    }
}
