using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileIconFactoryTests
{
    [Fact]
    public void Get_ReturnsBuiltInIcons()
    {
        var icon = FileIconFactory.Get(FileIconKey.Excel, FileView.SmallIcons);

        Assert.IsType<FileIcons.Size24.ExcelIcon>(icon);
    }

    [Fact]
    public void Get_UsesOverridesWhenPresent()
    {
        var custom = new FileIcons.Size24.DefaultFileIcon();
        var key = FileIconKey.Create("override-test");

        FileIconFactory.Override(key, FileView.SmallIcons, custom);
        var icon = FileIconFactory.Get(key, FileView.SmallIcons);

        Assert.Same(custom, icon);
    }

    [Fact]
    public void Get_ReturnsDefaultIconForUnknownKey()
    {
        var icon = FileIconFactory.Get(FileIconKey.Create("unknown"), FileView.SmallIcons);

        Assert.IsType<FileIcons.Size24.DefaultFileIcon>(icon);
        Assert.IsAssignableFrom<Icon>(icon);
    }
}
