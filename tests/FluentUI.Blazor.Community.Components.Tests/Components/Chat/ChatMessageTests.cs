namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageTests
{
    [Fact]
    public void Properties_SetAndGet_ReturnsExpectedValues()
    {
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var replyUser = new ChatUser { Id = 2, DisplayName = "Bob" };
        var replyMessage = new ChatMessage { Id = 99, Sender = replyUser };

        var message = new ChatMessage
        {
            Id = 42,
            ReplyMessageId = 99,
            CreatedDate = new DateTime(2025, 8, 6),
            Sender = user,
            MessageType = ChatMessageType.Text,
            IsPinned = true,
            Edited = true,
            IsDeleted = false,
            ReplyMessage = replyMessage
        };

        Assert.Equal(42, message.Id);
        Assert.Equal(99, message.ReplyMessageId);
        Assert.Equal(new DateTime(2025, 8, 6), message.CreatedDate);
        Assert.Equal(user, message.Sender);
        Assert.Equal(ChatMessageType.Text, message.MessageType);
        Assert.True(message.IsPinned);
        Assert.True(message.Edited);
        Assert.False(message.IsDeleted);
        Assert.Equal(replyMessage, message.ReplyMessage);
        Assert.NotNull(message.Sections);
        Assert.NotNull(message.Files);
        Assert.NotNull(message.Reactions);
        Assert.NotNull(message.ReadStates);
    }

    [Fact]
    public void SetReadState_UpdatesReadState_WhenUserExists()
    {
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var message = new ChatMessage();
        message.ReadStates[user] = false;

        message.SetReadState(user, true);

        Assert.True(message.ReadStates[user]);
    }

    [Fact]
    public void SetReadState_DoesNothing_WhenUserDoesNotExist()
    {
        var user = new ChatUser { Id = 1, DisplayName = "Alice" };
        var message = new ChatMessage();

        // Ne doit pas lever d'exception ni ajouter l'utilisateur
        message.SetReadState(user, true);

        Assert.False(message.ReadStates.ContainsKey(user));
    }

    [Fact]
    public void GetMessageReadState_ReturnsUnread_WhenAllOthersUnread()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };
        var message = new ChatMessage();
        message.ReadStates[user1] = false;
        message.ReadStates[user2] = false;

        var state = message.GetMessageReadState(user1);

        Assert.Equal(ChatMessageReadState.Unread, state);
    }

    [Fact]
    public void GetMessageReadState_ReturnsReadByEveryone_WhenAllOthersRead()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };
        var message = new ChatMessage();
        message.ReadStates[user1] = true;
        message.ReadStates[user2] = true;

        var state = message.GetMessageReadState(user1);

        Assert.Equal(ChatMessageReadState.ReadByEveryone, state);
    }

    [Fact]
    public void GetMessageReadState_ReturnsRead_WhenMixedStates()
    {
        var user1 = new ChatUser { Id = 1 };
        var user2 = new ChatUser { Id = 2 };
        var user3 = new ChatUser { Id = 3 };
        var message = new ChatMessage();
        message.ReadStates[user1] = true;
        message.ReadStates[user2] = false;
        message.ReadStates[user3] = true;

        var state = message.GetMessageReadState(user1);

        Assert.Equal(ChatMessageReadState.Read, state);
    }
}
