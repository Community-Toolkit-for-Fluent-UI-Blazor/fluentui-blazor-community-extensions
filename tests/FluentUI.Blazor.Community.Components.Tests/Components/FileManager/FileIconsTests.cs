using System.Text;
using Microsoft.FluentUI.AspNetCore.Components;
using static FluentUI.Blazor.Community.Components.FileIcons;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileIconsTests
{
    [Fact]
    public void FromExtension_ReturnsExpectedIcon_ForKnownExtension()
    {
        var icon = FromExtension(".xlsx");
        Assert.IsType<Size32.ExcelIcon>(icon);
    }

    [Fact]
    public void FromExtension_ReturnsDefaultIcon_ForUnknownExtension()
    {
        var icon = FromExtension(".unknownext");
        Assert.IsType<Size32.DefaultFileIcon>(icon);
    }

    [Fact]
    public void FromExtension_ReturnsDefaultIcon_ForNullOrEmpty()
    {
        Assert.IsType<Size32.DefaultFileIcon>(FromExtension(null));
        Assert.IsType<Size32.DefaultFileIcon>(FromExtension(""));
    }

    [Fact]
    public void FromExtensionAndGridViewOptions_ReturnsExpectedIcon()
    {
        var icon = FromExtensionAndGridViewOptions(".xlsx", FileView.Mosaic);
        Assert.IsType<Size32.ExcelIcon>(icon);
    }

    [Fact]
    public void FromExtensionAndGridViewOptions_ReturnsDefaultIcon_ForUnknownExtension()
    {
        var icon = FromExtensionAndGridViewOptions(".unknown", FileView.Mosaic);
        Assert.IsType<Size32.DefaultFileIcon>(icon);
    }

    [Fact]
    public void FromExtensionAndGridViewOptions_ReturnsDefaultIcon_ForNullOrEmpty()
    {
        Assert.IsType<Size32.DefaultFileIcon>(FromExtensionAndGridViewOptions(null, FileView.Mosaic));
        Assert.IsType<Size32.DefaultFileIcon>(FromExtensionAndGridViewOptions("", FileView.Mosaic));
    }

    [Fact]
    public void GetIconForDetails_ReturnsFolderIcon_IfIsDirectory()
    {
        var icon = GetIconForDetails(".xlsx", true);
        Assert.IsType<Size128.FolderIcon>(icon);
    }

    [Fact]
    public void GetIconForDetails_ReturnsExpectedIcon_ForKnownExtension()
    {
        var icon = GetIconForDetails(".xlsx", false);
        Assert.IsType<Size128.ExcelIcon>(icon);
    }

    [Fact]
    public void GetIconForDetails_ReturnsDefaultIcon_ForUnknownExtension()
    {
        var icon = GetIconForDetails(".unknown", false);
        Assert.IsType<Size128.DefaultFileIcon>(icon);
    }

    [Fact]
    public void GetIconForDetails_ReturnsDefaultIcon_ForNullOrEmpty()
    {
        Assert.IsType<Size128.DefaultFileIcon>(GetIconForDetails(null, false));
        Assert.IsType<Size128.DefaultFileIcon>(GetIconForDetails("", false));
    }

    [Fact]
    public void ToImageSource_ReturnsBase64String()
    {
        var icon = new TestIcon("svg-content");
        var src = FileIcons.ToImageSource(icon);
        Assert.StartsWith("data:image/svg+xml;base64,", src);
        var base64 = src["data:image/svg+xml;base64,".Length..];
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        Assert.Equal("svg-content", decoded);
    }

    // Classe de test pour Icon
    private class TestIcon : Icon
    {
        public TestIcon(string content) : base("TestIcon", IconVariant.Regular, IconSize.Size32, content) { }
    }
}
