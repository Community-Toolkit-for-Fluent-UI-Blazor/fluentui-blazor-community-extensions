using FluentUI.Blazor.Community.Components.FileManager;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class UploadFileEventArgsTests
{
    [Fact]
    public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "test.txt";
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var extension = ".txt";

        // Act
        var eventArgs = new UploadFileEventArgs(name, data, extension);

        // Assert
        Assert.Equal(name, eventArgs.Name);
        Assert.Equal(data, eventArgs.Data);
        Assert.Equal(extension, eventArgs.Extension);
    }

    [Fact]
    public void Constructor_WithNullName_SetsPropertyToNull()
    {
        // Arrange
        string? name = null;
        var data = new byte[] { 1, 2, 3 };
        var extension = ".txt";

        // Act
        var eventArgs = new UploadFileEventArgs(name!, data, extension);

        // Assert
        Assert.Null(eventArgs.Name);
        Assert.Equal(data, eventArgs.Data);
        Assert.Equal(extension, eventArgs.Extension);
    }

    [Fact]
    public void Constructor_WithEmptyData_SetsPropertyToEmptyArray()
    {
        // Arrange
        var name = "empty.txt";
        var data = Array.Empty<byte>();
        var extension = ".txt";

        // Act
        var eventArgs = new UploadFileEventArgs(name, data, extension);

        // Assert
        Assert.Equal(name, eventArgs.Name);
        Assert.Empty(eventArgs.Data);
        Assert.Equal(extension, eventArgs.Extension);
    }

    [Fact]
    public void Constructor_WithNullExtension_SetsPropertyToNull()
    {
        // Arrange
        var name = "test";
        var data = new byte[] { 1, 2, 3 };
        string? extension = null;

        // Act
        var eventArgs = new UploadFileEventArgs(name, data, extension!);

        // Assert
        Assert.Equal(name, eventArgs.Name);
        Assert.Equal(data, eventArgs.Data);
        Assert.Null(eventArgs.Extension);
    }

    [Fact]
    public void Equality_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var name = "test.txt";
        var data = new byte[] { 1, 2, 3 };
        var extension = ".txt";

        var eventArgs1 = new UploadFileEventArgs(name, data, extension);
        var eventArgs2 = new UploadFileEventArgs(name, data, extension);

        // Act & Assert
        Assert.Equal(eventArgs1, eventArgs2);
    }

    [Fact]
    public void Equality_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var eventArgs1 = new UploadFileEventArgs("test1.txt", new byte[] { 1, 2, 3 }, ".txt");
        var eventArgs2 = new UploadFileEventArgs("test2.txt", new byte[] { 4, 5, 6 }, ".pdf");

        // Act & Assert
        Assert.NotEqual(eventArgs1, eventArgs2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var name = "test.txt";
        var data = new byte[] { 1, 2, 3 };
        var extension = ".txt";

        var eventArgs1 = new UploadFileEventArgs(name, data, extension);
        var eventArgs2 = new UploadFileEventArgs(name, data, extension);

        // Act & Assert
        Assert.Equal(eventArgs1.GetHashCode(), eventArgs2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        // Arrange
        var name = "test.txt";
        var data = new byte[] { 1, 2, 3 };
        var extension = ".txt";
        var eventArgs = new UploadFileEventArgs(name, data, extension);

        // Act
        var result = eventArgs.ToString();

        // Assert
        Assert.Contains(name, result);
        Assert.Contains(extension, result);
    }
}