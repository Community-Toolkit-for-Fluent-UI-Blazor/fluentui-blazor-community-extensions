using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class BinaryChatFileTests
{
    [Fact]
    public void Constructor_InitializesDataToEmptyArray()
    {
        var file = new BinaryChatFile();
        Assert.NotNull(file.Data);
        Assert.Empty(file.Data);
    }

    [Fact]
    public void Data_SetAndGet_ReturnsExpectedValue()
    {
        var file = new BinaryChatFile();
        var bytes = new byte[] { 1, 2, 3, 4 };
        file.Data = bytes;
        Assert.Equal(bytes, file.Data);
    }

    [Fact]
    public void InheritedProperties_SetAndGet_ReturnsExpectedValues()
    {
        var user = new ChatUser { Id = 42, DisplayName = "Test" };
        var file = new BinaryChatFile
        {
            Id = 100,
            MessageId = 200,
            CreatedDate = new DateTime(2025, 8, 6),
            ContentType = "image/png",
            Name = "photo.png",
            Length = 12345,
            Owner = user
        };

        Assert.Equal(100, file.Id);
        Assert.Equal(200, file.MessageId);
        Assert.Equal(new DateTime(2025, 8, 6), file.CreatedDate);
        Assert.Equal("image/png", file.ContentType);
        Assert.Equal("photo.png", file.Name);
        Assert.Equal(12345, file.Length);
        Assert.Equal(user, file.Owner);
    }
}
