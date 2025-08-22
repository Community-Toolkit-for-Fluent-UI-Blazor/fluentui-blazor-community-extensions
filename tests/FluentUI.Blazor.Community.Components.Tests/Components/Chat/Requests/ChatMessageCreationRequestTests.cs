using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatMessageCreationRequestTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        // Arrange
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var draft = new ChatMessageDraft { Text = "Hello" };
        var splitOption = ChatMessageSplitOption.Split;
        var isTranslationEnabled = true;
        long roomId = 42;

        // Act
        var request = new ChatMessageCreationRequest(roomId, user, draft, splitOption, isTranslationEnabled);

        // Assert
        Assert.Equal(roomId, request.RoomId);
        Assert.Equal(user, request.Owner);
        Assert.Equal(draft, request.ChatDraft);
        Assert.Equal(splitOption, request.SplitOption);
        Assert.True(request.IsTranslationEnabled);
    }

    [Fact]
    public void Records_WithSameValues_AreEqual()
    {
        // Arrange
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var draft = new ChatMessageDraft { Text = "Hello" };
        var splitOption = ChatMessageSplitOption.None;
        var isTranslationEnabled = false;
        long roomId = 99;

        // Act
        var req1 = new ChatMessageCreationRequest(roomId, user, draft, splitOption, isTranslationEnabled);
        var req2 = new ChatMessageCreationRequest(roomId, user, draft, splitOption, isTranslationEnabled);

        // Assert
        Assert.Equal(req1, req2);
        Assert.True(req1 == req2);
        Assert.False(req1 != req2);
        Assert.Equal(req1.GetHashCode(), req2.GetHashCode());
    }

    [Fact]
    public void Deconstruct_ReturnsAllProperties()
    {
        // Arrange
        var user = new ChatUser { Id = 2, DisplayName = "Bob" };
        var draft = new ChatMessageDraft { Text = "Test" };
        var splitOption = ChatMessageSplitOption.Split;
        var isTranslationEnabled = true;
        long roomId = 123;

        var request = new ChatMessageCreationRequest(roomId, user, draft, splitOption, isTranslationEnabled);

        // Act
        var (actualRoomId, actualOwner, actualDraft, actualSplitOption, actualIsTranslationEnabled) = request;

        // Assert
        Assert.Equal(roomId, actualRoomId);
        Assert.Equal(user, actualOwner);
        Assert.Equal(draft, actualDraft);
        Assert.Equal(splitOption, actualSplitOption);
        Assert.Equal(isTranslationEnabled, actualIsTranslationEnabled);
    }
}
