using FluentUI.Blazor.Community.Components.TrailMenu;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuIconsTests
{
    [Fact]
    public void Icons_AreInstantiated()
    {
        Assert.NotNull(TrailMenuIcons.ChevronRight);
        Assert.NotNull(TrailMenuIcons.More);
        Assert.NotNull(TrailMenuIcons.ChevronDown);
        Assert.NotNull(TrailMenuIcons.HomeIcon);
        Assert.NotNull(TrailMenuIcons.DesktopIcon);
        Assert.NotNull(TrailMenuIcons.PhoneIcon);
    }

    [Fact]
    public void Icons_AreCorrectTypes()
    {
        Assert.IsType<Size24.ChevronRight>(TrailMenuIcons.ChevronRight);
        Assert.IsType<Size24.MoreHorizontal>(TrailMenuIcons.More);
        Assert.IsType<Size24.ChevronDown>(TrailMenuIcons.ChevronDown);
        Assert.IsType<Size24.Home>(TrailMenuIcons.HomeIcon);
        Assert.IsType<Size24.Desktop>(TrailMenuIcons.DesktopIcon);
        Assert.IsType<Size24.Phone>(TrailMenuIcons.PhoneIcon);
    }
}
