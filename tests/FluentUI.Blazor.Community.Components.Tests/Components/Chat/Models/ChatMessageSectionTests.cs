using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageSectionTests
{
    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        // Arrange
        var section = new ChatMessageSection();

        var id = 123L;
        var messageId = 456L;
        var cultureId = 789L;
        var content = "Contenu de test";
        var createdDate = new DateTime(2024, 6, 1, 12, 0, 0);

        // Act
        section.Id = id;
        section.MessageId = messageId;
        section.CultureId = cultureId;
        section.Content = content;
        section.CreatedDate = createdDate;

        // Assert
        Assert.Equal(id, section.Id);
        Assert.Equal(messageId, section.MessageId);
        Assert.Equal(cultureId, section.CultureId);
        Assert.Equal(content, section.Content);
        Assert.Equal(createdDate, section.CreatedDate);
    }

    [Fact]
    public void Implements_IChatMessageSection()
    {
        // Arrange
        var section = new ChatMessageSection();

        // Act & Assert
        Assert.IsAssignableFrom<IChatMessageSection>(section);
    }
}
