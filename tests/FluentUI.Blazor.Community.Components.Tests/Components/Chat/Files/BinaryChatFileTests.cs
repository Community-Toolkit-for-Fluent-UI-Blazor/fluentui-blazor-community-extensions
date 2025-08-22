using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class BinaryChatFileTests
{
    [Fact]
    public void Data_Property_Should_Set_And_Get_Value()
    {
        // Arrange
        var file = new BinaryChatFile();
        var data = new byte[] { 1, 2, 3 };

        // Act
        file.Data = data;

        // Assert
        Assert.Equal(data, file.Data);
    }

    [Fact]
    public void Inherited_Properties_Should_Set_And_Get_Values()
    {
        // Arrange
        var file = new BinaryChatFile();
        var now = DateTime.UtcNow;
        var owner = new ChatUser { Id = 42, DisplayName = "Test" };

        // Act
        file.Id = 1;
        file.MessageId = 2;
        file.CreatedDate = now;
        file.ContentType = "application/pdf";
        file.Name = "file.pdf";
        file.Length = 12345;
        file.Owner = owner;

        // Assert
        Assert.Equal(1, file.Id);
        Assert.Equal(2, file.MessageId);
        Assert.Equal(now, file.CreatedDate);
        Assert.Equal("application/pdf", file.ContentType);
        Assert.Equal("file.pdf", file.Name);
        Assert.Equal(12345, file.Length);
        Assert.Equal(owner, file.Owner);
    }
}
