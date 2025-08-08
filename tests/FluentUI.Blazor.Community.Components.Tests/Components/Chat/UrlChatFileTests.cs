using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class UrlChatFileTests
{
    [Fact]
    public void Constructor_InitializesUrlToDefault()
    {
        var file = new UrlChatFile();
        Assert.Null(file.Url);
    }

    [Fact]
    public void Url_SetAndGet_ReturnsExpectedValue()
    {
        var file = new UrlChatFile();
        var url = "https://example.com/file.pdf";
        file.Url = url;
        Assert.Equal(url, file.Url);
    }

    [Fact]
    public void InheritedProperties_SetAndGet_ReturnsExpectedValues()
    {
        var user = new ChatUser { Id = 7, DisplayName = "Alice" };
        var file = new UrlChatFile
        {
            Id = 101,
            MessageId = 202,
            CreatedDate = new DateTime(2025, 8, 6),
            ContentType = "application/pdf",
            Name = "file.pdf",
            Length = 54321,
            Owner = user
        };

        Assert.Equal(101, file.Id);
        Assert.Equal(202, file.MessageId);
        Assert.Equal(new DateTime(2025, 8, 6), file.CreatedDate);
        Assert.Equal("application/pdf", file.ContentType);
        Assert.Equal("file.pdf", file.Name);
        Assert.Equal(54321, file.Length);
        Assert.Equal(user, file.Owner);
    }
}
