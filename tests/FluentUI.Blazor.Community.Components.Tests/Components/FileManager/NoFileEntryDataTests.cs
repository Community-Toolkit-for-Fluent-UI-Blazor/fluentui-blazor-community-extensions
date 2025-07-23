using FluentUI.Blazor.Community.Components.FileManager;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class NoFileEntryDataTests
{
    [Fact]
    public void NoFileEntryData_DefaultBehavior_AllowsAllOperations()
    {
        // Arrange & Act
        var noFileEntry = new NoFileEntryData();

        // Assert
        Assert.NotNull(noFileEntry);
        Assert.True(noFileEntry.IsDownloadAllowed);
        Assert.True(noFileEntry.IsRenamable);
        Assert.True(noFileEntry.IsDeleteable);
    }

    [Fact]
    public void NoFileEntryData_ImplementsRequiredInterfaces()
    {
        // Arrange & Act
        var noFileEntry = new NoFileEntryData();

        // Assert
        Assert.IsAssignableFrom<IDownloadable>(noFileEntry);
        Assert.IsAssignableFrom<IRenamable>(noFileEntry);
        Assert.IsAssignableFrom<IDeletable>(noFileEntry);
    }
}