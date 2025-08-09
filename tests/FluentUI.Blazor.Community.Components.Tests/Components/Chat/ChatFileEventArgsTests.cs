using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatFileEventArgsTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 123;
        var name = "document.pdf";
        var contentType = "application/pdf";
        Func<Task<byte[]>> dataFunc = () => Task.FromResult(new byte[] { 1, 2, 3 });

        // Act
        var args = new ChatFileEventArgs(id, name, contentType, dataFunc);

        // Assert
        Assert.Equal(id, args.Id);
        Assert.Equal(name, args.Name);
        Assert.Equal(contentType, args.ContentType);
        Assert.Equal(dataFunc, args.DataFunc);
    }

    [Fact]
    public async Task Data_ReturnsExpectedBytes()
    {
        // Arrange
        var expectedBytes = new byte[] { 10, 20, 30 };
        Func<Task<byte[]>> dataFunc = () => Task.FromResult(expectedBytes);

        var args = new ChatFileEventArgs(123, "name", "type", dataFunc);

        // Act
        var result = await args.DataFunc!();

        // Assert
        Assert.Equal(expectedBytes, result);
    }
}
