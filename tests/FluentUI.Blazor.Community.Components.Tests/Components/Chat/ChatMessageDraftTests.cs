using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageDraftTests
{
    private class DummySection : IChatMessageSection
    {
        public long CultureId { get; set; }
        public string Content { get; set; } = string.Empty;
        public long Id { get; set; }
        public long MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    private class DummyMessage : IChatMessage
    {
        public long Id { get; set; }
        public long? ReplyMessageId => null;
        public DateTime CreatedDate => DateTime.Now;
        public ChatUser? Sender => null;
        public ChatMessageType MessageType => ChatMessageType.Text;
        public bool IsPinned => false;
        public bool Edited => false;
        public bool IsDeleted => false;
        public List<IChatMessageSection> Sections { get; set; } = new();
        public List<IChatFile> Files => new();
        public List<IChatMessageReaction> Reactions => new();
        public Dictionary<ChatUser, bool> ReadStates => new();
        public void SetReadState(ChatUser user, bool read) { }
        public ChatMessageReadState GetMessageReadState(ChatUser butUser) => ChatMessageReadState.Unread;
        public IChatMessage? ReplyMessage => null;
    }

    [Fact]
    public void AddCultureText_StoresCultureAndTexts()
    {
        var draft = new ChatMessageDraft();
        draft.AddCultureText("fr-FR", new[] { "Bonjour", "Salut" });
        var texts = draft.GetTranslatedTexts();
        Assert.True(texts.ContainsKey("fr-FR"));
        Assert.Contains("Bonjour", texts["fr-FR"]);
        Assert.Contains("Salut", texts["fr-FR"]);
    }

    [Fact]
    public void Clear_RemovesAllData()
    {
        var draft = new ChatMessageDraft
        {
            Text = "Test",
            SelectedChatFiles = new List<ChatFileEventArgs> { new(123, "name", "type", () => Task.FromResult(new byte[0])) }
        };
        draft.AddCultureText("en-US", new[] { "Hello" });
        draft.SetEditMessage(new ChatUser { Id = 1 }, new DummyMessage());
        draft.SetReplyMessage(new DummyMessage());

        draft.Clear();

        Assert.Empty(draft.GetTranslatedTexts());
        Assert.Equal(string.Empty, draft.Text);
        Assert.Empty(draft.SelectedChatFiles);
        Assert.Null(draft.GetEditMessage());
        Assert.Null(draft.Reply);
    }

    [Fact]
    public void SetEditMessage_SetsEditMessageAndText()
    {
        var owner = new ChatUser { CultureId = 10 };
        var msg = new DummyMessage
        {
            Sections = new List<IChatMessageSection>
            {
                new DummySection { CultureId = 10, Content = "Texte FR" },
                new DummySection { CultureId = 20, Content = "Texte EN" }
            }
        };
        var draft = new ChatMessageDraft();
        draft.SetEditMessage(owner, msg);

        Assert.Equal(msg, draft.GetEditMessage());
        Assert.Equal("Texte FR", draft.Text);
    }

    [Fact]
    public void ClearEditMessage_RemovesEditMessageAndText()
    {
        var draft = new ChatMessageDraft();
        draft.SetEditMessage(new ChatUser { CultureId = 1 }, new DummyMessage());
        draft.ClearEditMessage();
        Assert.Null(draft.GetEditMessage());
        Assert.Equal(string.Empty, draft.Text);
    }

    [Fact]
    public void SetReplyMessage_SetsReplyMessage()
    {
        var msg = new DummyMessage();
        var draft = new ChatMessageDraft();
        draft.SetReplyMessage(msg);
        Assert.Equal(msg, draft.Reply);
    }

    [Fact]
    public void ClearReplyMessage_RemovesReplyMessage()
    {
        var msg = new DummyMessage();
        var draft = new ChatMessageDraft();
        draft.SetReplyMessage(msg);
        draft.ClearReplyMessage();
        Assert.Null(draft.Reply);
    }

    [Fact]
    public void GetReplyText_ReturnsCorrectTextForOwnerCulture()
    {
        var owner = new ChatUser { CultureId = 20 };
        var msg = new DummyMessage
        {
            Sections = new List<IChatMessageSection>
            {
                new DummySection { CultureId = 10, Content = "FR" },
                new DummySection { CultureId = 20, Content = "EN" }
            }
        };
        var draft = new ChatMessageDraft();
        draft.SetReplyMessage(msg);
        var replyText = draft.GetReplyText(owner);
        Assert.Equal("EN", replyText);
    }

    [Fact]
    public void GetReplyText_ReturnsFirstSectionIfCultureNotFound()
    {
        var owner = new ChatUser { CultureId = 99 };
        var msg = new DummyMessage
        {
            Sections = new List<IChatMessageSection>
            {
                new DummySection { CultureId = 10, Content = "FR" },
                new DummySection { CultureId = 20, Content = "EN" }
            }
        };
        var draft = new ChatMessageDraft();
        draft.SetReplyMessage(msg);
        var replyText = draft.GetReplyText(owner);
        Assert.Equal("FR", replyText);
    }
}
