using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileIconKeyTests
{
    [Fact]
    public void Create_SetsKey()
    {
        var key = FileIconKey.Create("custom");

        Assert.Equal("custom", key.Key);
    }

    [Fact]
    public void BuiltInKeys_HaveExpectedValues()
    {
        Assert.Equal("default", FileIconKey.Default.Key);
        Assert.Equal("folder", FileIconKey.Folder.Key);
        Assert.Equal("excel", FileIconKey.Excel.Key);
        Assert.Equal("word", FileIconKey.Word.Key);
        Assert.Equal("powerpoint", FileIconKey.PowerPoint.Key);
        Assert.Equal("image", FileIconKey.Image.Key);
        Assert.Equal("audio", FileIconKey.Audio.Key);
        Assert.Equal("video", FileIconKey.Video.Key);
        Assert.Equal("pdf", FileIconKey.Pdf.Key);
        Assert.Equal("json", FileIconKey.Json.Key);
        Assert.Equal("powerbi", FileIconKey.PowerBi.Key);
        Assert.Equal("program", FileIconKey.Program.Key);
        Assert.Equal("multiselection", FileIconKey.MultiSelection.Key);
    }
}
