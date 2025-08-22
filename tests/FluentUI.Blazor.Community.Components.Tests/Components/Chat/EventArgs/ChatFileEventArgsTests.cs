using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatFileEventArgsTests
{
    [Fact]
    public void Constructor_WithIdAndDataFunc_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 123;
        var name = "test.txt";
        var contentType = "text/plain";
        var isRecordedAudio = true;
        var expectedData = new byte[] { 1, 2, 3 };
        var dataFunc = () => Task.FromResult(expectedData);

        // Act
        var args = new ChatFileEventArgs(id, name, contentType, dataFunc, isRecordedAudio);

        // Assert
        Assert.Equal($"f{id}", args.Id);
        Assert.Equal(name, args.Name);
        Assert.Equal(contentType, args.ContentType);
        Assert.Equal(dataFunc, args.DataFunc);
        Assert.True(args.IsRecordedAudio);
        Assert.Empty(args.Data);
    }

    [Fact]
    public async Task DataFunc_ReturnsExpectedData()
    {
        // Arrange
        byte[] expectedData = [4, 5, 6];
        Func<Task<byte[]>> dataFunc = () => Task.FromResult(expectedData);
        var args = new ChatFileEventArgs(1, "file", "type", dataFunc);

        // Act
        var data = await args.DataFunc!();

        // Assert
        Assert.Equal(expectedData, data);
    }

    [Fact]
    public void Constructor_WithData_SetsPropertiesCorrectly()
    {
        // Arrange
        string name = "audio.mp3";
        string contentType = "audio/mpeg";
        byte[] data = [7, 8, 9];
        bool isRecordedAudio = true;

        // Act
        var args = new ChatFileEventArgs(name, contentType, data, isRecordedAudio);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(args.Id));
        Assert.Equal(name, args.Name);
        Assert.Equal(contentType, args.ContentType);
        Assert.Equal(data, args.Data);
        Assert.True(args.IsRecordedAudio);
        Assert.Null(args.DataFunc);
    }

    [Fact]
    public void Constructor_WithData_DefaultIsRecordedAudioFalse()
    {
        // Arrange
        string name = "file.bin";
        string contentType = "application/octet-stream";
        byte[] data = [10, 11];

        // Act
        var args = new ChatFileEventArgs(name, contentType, data);

        // Assert
        Assert.False(args.IsRecordedAudio);
    }

    [Fact]
    public void Constructor_WithIdAndDataFunc_DefaultIsRecordedAudioFalse()
    {
        // Arrange
        var dataFunc = () => Task.FromResult(new byte[] { 1 });
        var args = new ChatFileEventArgs(1, "file", "type", dataFunc);

        // Assert
        Assert.False(args.IsRecordedAudio);
    }
}
