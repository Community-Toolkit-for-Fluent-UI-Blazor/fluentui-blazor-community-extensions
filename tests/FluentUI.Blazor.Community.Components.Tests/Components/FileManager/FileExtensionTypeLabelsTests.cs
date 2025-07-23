namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileExtensionTypeLabelsTests
{
    [Fact]
    public void Default_Instance_HasEnglishLabels()
    {
        var labels = FileExtensionTypeLabels.Default;
        Assert.Equal("Windows Audio File", labels.WindowsAudioFile);
        Assert.Equal("Microsoft Access Database File", labels.MicrosoftAccessDatabaseFile);
        Assert.Equal("Unknown file", labels.UnknownValue);
        Assert.Equal("Json File", labels.JsonFile);
    }

    [Fact]
    public void French_Instance_HasFrenchLabels()
    {
        var labels = FileExtensionTypeLabels.French;
        Assert.Equal("Fichier audio Windows", labels.WindowsAudioFile);
        Assert.Equal("Fichier de base de données Microsoft Access", labels.MicrosoftAccessDatabaseFile);
        Assert.Equal("Fichier inconnu", labels.UnknownValue);
        Assert.Equal("Fichier Json", labels.JsonFile);
    }

    [Fact]
    public void Can_Set_Label_Properties()
    {
        var labels = FileExtensionTypeLabels.Default with
        {
            WindowsAudioFile = "Test Label"
        };

        Assert.Equal("Test Label", labels.WindowsAudioFile);
    }

    [Fact]
    public void All_Properties_Are_NotNull()
    {
        var labels = FileExtensionTypeLabels.Default;
        foreach (var prop in typeof(FileExtensionTypeLabels).GetProperties())
        {
            var value = prop.GetValue(labels);
            Assert.NotNull(value);
        }
    }
}
