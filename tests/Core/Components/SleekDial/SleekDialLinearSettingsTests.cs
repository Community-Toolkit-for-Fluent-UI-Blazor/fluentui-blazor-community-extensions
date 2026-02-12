using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialLinearSettingsTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var settings = new SleekDialLinearSettings();

        Assert.Equal("12px", settings.ItemOffset);
        Assert.Equal("4px", settings.Gap);
    }

    [Fact]
    public void ItemOffset_CanBeUpdated()
    {
        var settings = new SleekDialLinearSettings();

        settings.ItemOffset = "20px";

        Assert.Equal("20px", settings.ItemOffset);
    }

    [Fact]
    public void Gap_CanBeUpdated()
    {
        var settings = new SleekDialLinearSettings();

        settings.Gap = "8px";

        Assert.Equal("8px", settings.Gap);
    }
}
