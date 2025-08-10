using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageCreationRequestTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var user = new ChatUser
        {
            Id = 1,
            DisplayName = "Alice",
            Avatar = "avatar.png",
            Initials = "A",
            CultureId = 10,
            CultureName = "fr-FR",
            Roles = new List<string> { "admin" }
        };

        var draft = new ChatMessageDraft
        {
            Text = "Bonjour",
            SelectedChatFiles = new List<ChatFileEventArgs>()
        };

        var request = new ChatMessageCreationRequest(
            42,
            user,
            draft,
            ChatMessageSplitOption.Split,
            true
        );

        Assert.Equal(42, request.RoomId);
        Assert.Equal(user, request.Owner);
        Assert.Equal(draft, request.ChatDraft);
        Assert.Equal(ChatMessageSplitOption.Split, request.SplitOption);
        Assert.True(request.IsTranslationEnabled);
    }

    [Fact]
    public void CanUseSplitOptionNone()
    {
        var user = new ChatUser { Id = 2, DisplayName = "Bob", Roles = [] };
        var draft = new ChatMessageDraft { Text = "Salut", SelectedChatFiles = [] };

        var request = new ChatMessageCreationRequest(99, user, draft, ChatMessageSplitOption.None, true);

        Assert.Equal(ChatMessageSplitOption.None, request.SplitOption);
    }

    [Fact]
    public void CanDisableIsTranslationEnabled()
    {
        var user = new ChatUser { Id = 2, DisplayName = "Bob", Roles = [] };
        var draft = new ChatMessageDraft { Text = "Salut", SelectedChatFiles = [] };

        var request = new ChatMessageCreationRequest(99, user, draft, ChatMessageSplitOption.None, false);

        Assert.False(request.IsTranslationEnabled);
    }
}
