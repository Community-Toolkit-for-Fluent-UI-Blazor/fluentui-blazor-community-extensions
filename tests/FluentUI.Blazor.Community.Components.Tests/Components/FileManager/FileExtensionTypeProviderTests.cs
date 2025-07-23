namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileExtensionTypeProviderTests
{
    [Fact]
    public void Get_ReturnsLabel_ForKnownExtension_DefaultLabels()
    {
        var provider = new FileExtensionTypeProvider(FileExtensionTypeLabels.Default);
        var label = provider.Get(".xlsx");
        Assert.Equal(FileExtensionTypeLabels.Default.MicrosoftExcelWorkbook, label);
    }

    [Fact]
    public void Get_ReturnsLabel_ForKnownExtension_FrenchLabels()
    {
        var provider = new FileExtensionTypeProvider(FileExtensionTypeLabels.French);
        var label = provider.Get(".xlsx");
        Assert.Equal(FileExtensionTypeLabels.French.MicrosoftExcelWorkbook, label);
    }

    [Fact]
    public void Get_ReturnsUnknown_ForUnknownExtension()
    {
        var provider = new FileExtensionTypeProvider(FileExtensionTypeLabels.Default);
        var label = provider.Get(".unknownext");
        Assert.Equal(FileExtensionTypeLabels.Default.UnknownValue, label);
    }

    [Fact]
    public void Get_ReturnsEmpty_ForNullOrEmpty()
    {
        var provider = new FileExtensionTypeProvider(FileExtensionTypeLabels.Default);
        Assert.Equal(string.Empty, provider.Get(null));
        Assert.Equal(string.Empty, provider.Get(""));
    }

    [Fact]
    public void Get_IsCaseInsensitive()
    {
        var provider = new FileExtensionTypeProvider(FileExtensionTypeLabels.Default);
        var labelLower = provider.Get(".xlsx");
        var labelUpper = provider.Get(".XLSX");
        Assert.Equal(labelLower, labelUpper);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_IfLabelsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FileExtensionTypeProvider(null));
    }
}
