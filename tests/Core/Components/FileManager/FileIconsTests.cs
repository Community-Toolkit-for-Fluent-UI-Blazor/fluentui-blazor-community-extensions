using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileIconsTests
{
    [Fact]
    public void Size24Icons_AreInstantiated()
    {
        var icon = new FileIcons.Size24.ExcelIcon();

        Assert.IsAssignableFrom<Icon>(icon);
    }

    [Fact]
    public void Size128Icons_AreInstantiated()
    {
        var icon = new FileIcons.Size128.ProgramIcon();

        Assert.IsAssignableFrom<Icon>(icon);
    }
}
