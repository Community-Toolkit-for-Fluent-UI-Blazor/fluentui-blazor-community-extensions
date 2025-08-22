using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class UrlChatFileTests
{
    [Fact]
    public void Url_Property_GetSet_Works()
    {
        // Arrange
        var file = new UrlChatFile();

        // Act
        file.Url = "https://example.com/file.txt";

        // Assert
        Assert.Equal("https://example.com/file.txt", file.Url);
    }

    [Fact]
    public void Inherited_Properties_GetSet_Works()
    {
        // Arrange
        var file = new UrlChatFile();
        var owner = new ChatUser { Id = 42, DisplayName = "Test User" };
        var now = DateTime.UtcNow;

        // Act
        file.Id = 1;
        file.MessageId = 2;
        file.CreatedDate = now;
        file.ContentType = "text/plain";
        file.Name = "file.txt";
        file.Length = 1234;
        file.Owner = owner;

        // Assert
        Assert.Equal(1, file.Id);
        Assert.Equal(2, file.MessageId);
        Assert.Equal(now, file.CreatedDate);
        Assert.Equal("text/plain", file.ContentType);
        Assert.Equal("file.txt", file.Name);
        Assert.Equal(1234, file.Length);
        Assert.Equal(owner, file.Owner);
    }
}
