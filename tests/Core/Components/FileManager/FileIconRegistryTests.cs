using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileIconRegistryTests
{
    [Fact]
    public void Resolve_ReturnsDefaultForNull()
    {
        var key = FileIconRegistry.Resolve(null);

        Assert.Equal(FileIconKey.Default, key);
    }

    [Fact]
    public void Resolve_ReturnsMappedKey()
    {
        var key = FileIconRegistry.Resolve(".xlsx");

        Assert.Equal(FileIconKey.Excel, key);
    }

    [Fact]
    public void Register_AddsLeadingDotAndIsCaseInsensitive()
    {
        FileIconRegistry.Register("custom", FileIconKey.Program);

        Assert.Equal(FileIconKey.Program, FileIconRegistry.Resolve(".CUSTOM"));
    }
}
