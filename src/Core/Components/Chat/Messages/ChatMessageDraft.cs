using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a draf for a chat message.
/// </summary>
public sealed class ChatMessageDraft
{
    /// <summary>
    /// Represents the message to edit.
    /// </summary>
    private ChatMessage? _editMessage;

    /// <summary>
    /// Represents the replied message.
    /// </summary>
    private ChatMessage? _replyMessage;

    /// <summary>
    /// Represents all cultures to convert a message.
    /// </summary>
    private readonly Dictionary<string, IEnumerable<string>> _textCultures = [];

    /// <summary>
    /// Gets or sets the text of the message.
    /// </summary>
    public string? Text { get; internal set; }

    /// <summary>
    /// Gets or sets the files to send with the message.
    /// </summary>
    public List<ChatFileEventArgs> SelectedChatFiles { get; internal set; } = [];

    /// <summary>
    /// Gets or sets the identifier of the sender of the message.
    /// </summary>
    public long SenderId { get; internal set; }

    /// <summary>
    /// Gets the replied message.
    /// </summary>
    public ChatMessage? Reply => _replyMessage;

    /// <summary>
    /// Adds a culture of the translated texts.
    /// </summary>
    /// <param name="culture">Culture of the message.</param>
    /// <param name="texts">Translated texts into the culture.</param>
    internal void AddCultureText(string culture, IEnumerable<string> texts)
    {
        _textCultures.TryAdd(culture, texts);
    }

    /// <summary>
    /// Gets all texts ordered by cultures.
    /// </summary>
    /// <returns>Returns all texts in their own culture.</returns>
    public IReadOnlyDictionary<string, IEnumerable<string>> GetTranslatedTexts()
    {
        return _textCultures;
    }

    /// <summary>
    /// Clear the draft.
    /// </summary>
    internal void Clear()
    {
        _textCultures.Clear();
        Text = string.Empty;
        SelectedChatFiles.Clear();
        ClearEditMessage();
        ClearReplyMessage();
    }

    /// <summary>
    /// Remove the replied message.
    /// </summary>
    internal void ClearReplyMessage()
    {
        _replyMessage = null;
    }

    /// <summary>
    /// Clear the edited message.
    /// </summary>
    internal void ClearEditMessage()
    {
        _editMessage = null;
        Text = string.Empty;
    }

    /// <summary>
    /// Gets the edited message.
    /// </summary>
    /// <returns>Returns the edited message.</returns>
    internal ChatMessage? GetEditMessage()
    {
        return _editMessage;
    }

    /// <summary>
    /// Gets the replied message from the culture of the owner.
    /// </summary>
    /// <param name="owner">Owner of the message to reply.</param>
    /// <param name="localizer">Localizer to get the default reply text.</param>
    /// <returns>Returns the reply message of the owner.</returns>
    internal string? GetReplyText(ChatUser? owner, IFluentLocalizer localizer)
    {
        if (_replyMessage is null)
        {
            return null;
        }

        if (_replyMessage.Type == ChatMessageType.Text)
        {
            var section = _replyMessage.Sections.FirstOrDefault(x => x.CultureId == owner?.CultureId);

            section ??= _replyMessage.Sections.Count > 0 ? _replyMessage.Sections[0] : null;

            return section?.Content;
        }
        else if (_replyMessage.Type == ChatMessageType.Files)
        {
            return localizer[LanguageResource.CX_Chat_Message_ReplyFromDocumentOnly];
        }
        else if (_replyMessage.Type == ChatMessageType.Gift)
        {
            return localizer[LanguageResource.CX_Chat_Message_ReplyFromGiftOnly];
        }
        else
        {
            return localizer[LanguageResource.CX_Chat_Message_ReplyFromMultipleSources];
        }
    }

    /// <summary>
    /// Sets the message to edit from the culture of the owner.
    /// </summary>
    /// <param name="owner">Owner of the message.</param>
    /// <param name="message">Message to edit.</param>
    internal void SetEditMessage(ChatUser owner, ChatMessage message)
    {
        _editMessage = message;
        Text = message.Sections.FirstOrDefault(s => s.CultureId == owner.CultureId)?.Content;
    }

    /// <summary>
    /// Sets the reply message.
    /// </summary>
    /// <param name="message">Message to reply.</param>
    internal void SetReplyMessage(ChatMessage message)
    {
        _replyMessage = message;
    }

    internal Task<ChatMessageBuildResult> BuildAsync(
        long roomId,
        ChatUser sender,
        ChatMessageSplitOption messageSplitOption)
    {
        return messageSplitOption switch
        {
            ChatMessageSplitOption.None => BuildSingleMessageAsync(roomId, sender),
            ChatMessageSplitOption.SplitTextAndDocument => BuildMultipleMessagesAsync(roomId, sender),
            _ => throw new NotSupportedException("The ChatMessageSplitOption value is not recognized.")
        };
    }

    private async Task<ChatMessageBuildResult> BuildMultipleMessagesAsync(
        long roomId,
        ChatUser sender)
    {
        var result = new List<ChatMessageBuildItem>();
        var texts = GetTranslatedTexts();

        if (texts.Count > 0)
        {
            var textMessage = new ChatMessage()
            {
                Id = FluentCxConstants.NewTextMessageIdentifier,
                CreatedDate = DateTime.UtcNow,
                RoomId = roomId,
                SenderId = sender.Id,
                ReplyToMessageId = Reply?.Id,
                ReplyToMessage = Reply,
                Type = ChatMessageType.Text,
                Sections = [.. texts.Select(t => new ChatMessageSection()
                {
                    Id = -1,
                    CreatedDate = DateTime.UtcNow,
                    CultureName = t.Key,
                    CultureId = sender.CultureId,
                    MessageId = -1,
                    Content = string.Join(Environment.NewLine, t.Value)
                })]
            };

            result.Add(new ChatMessageBuildItem(textMessage, []));
        }

        // Split messages works like that :
        // * Audio ! One message each file.
        // * Images : One message for all image files.
        // * Videos : One message for all video files.
        // * Other files : One message for all other files.

        var audioFiles = SelectedChatFiles.Where(f => f.ContentType.StartsWith(FluentCxConstants.AudioContentType, StringComparison.OrdinalIgnoreCase));

        if (audioFiles.Any())
        {
            foreach (var audioFile in audioFiles)
            {
                await BuildAsync(ChatMessageType.Audio, [audioFile]);
            }
        }

        var videoFiles = SelectedChatFiles.Where(f => f.ContentType.StartsWith(FluentCxConstants.VideoContentType, StringComparison.OrdinalIgnoreCase));

        if (videoFiles.Any())
        {
            await BuildAsync(ChatMessageType.Videos, videoFiles);
        }

        var imageFiles = SelectedChatFiles.Where(f => f.ContentType.StartsWith(FluentCxConstants.ImageContentType, StringComparison.OrdinalIgnoreCase));

        if (imageFiles.Any())
        {
            await BuildAsync(ChatMessageType.Images, imageFiles);
        }

        var otherFiles = SelectedChatFiles.Where(f => !f.ContentType.StartsWith(FluentCxConstants.AudioContentType, StringComparison.OrdinalIgnoreCase)
            && !f.ContentType.StartsWith(FluentCxConstants.VideoContentType, StringComparison.OrdinalIgnoreCase)
            && !f.ContentType.StartsWith(FluentCxConstants.ImageContentType, StringComparison.OrdinalIgnoreCase));

        if (otherFiles.Any())
        {
            await BuildAsync(ChatMessageType.Files, otherFiles);
        }

        async Task BuildAsync(ChatMessageType type, IEnumerable<ChatFileEventArgs> e)
        {
            var messageIdentifier = type switch
            {
                ChatMessageType.Audio => FluentCxConstants.NewAudioMessageIdentifier,
                ChatMessageType.Videos => FluentCxConstants.NewVideoMessageIdentifier,
                ChatMessageType.Images => FluentCxConstants.NewImageMessageIdentifier,
                ChatMessageType.Files => FluentCxConstants.NewFileMessageIdentifier,
                _ => throw new NotSupportedException("The ChatMessageType value is not recognized.")
            };

            var message = new ChatMessage()
            {
                Id = messageIdentifier,
                CreatedDate = DateTime.UtcNow,
                RoomId = roomId,
                SenderId = sender.Id,
                ReplyToMessageId = Reply?.Id,
                ReplyToMessage = Reply,
                Type = type,
                Sections = []
            };

            var files = new List<IBinaryChatFile>();

            foreach (var item in e)
            {
                var content = await item.GetDataAsync();

                var file = new BinaryChatFile()
                {
                    Id = -2,
                    MessageId = messageIdentifier,
                    CreatedDate = DateTime.UtcNow,
                    Name = item.Name,
                    Content = content,
                    Length = content.Length,
                    ContentType = item.ContentType
                };

                files.Add(file);
            }

            result.Add(new ChatMessageBuildItem(message, files));
        }

        return new(result);
    }

    private async Task<ChatMessageBuildResult> BuildSingleMessageAsync(
        long roomId,
        ChatUser sender)
    {
        var type = ChatMessageType.None;
        var texts = GetTranslatedTexts();

        if (texts.Count > 0)
        {
            type |= ChatMessageType.Text;
        }

        if (SelectedChatFiles.FindIndex(f => f.ContentType.StartsWith(FluentCxConstants.AudioContentType, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            type |= ChatMessageType.Audio;
        }

        if (SelectedChatFiles.FindIndex(f => f.ContentType.StartsWith(FluentCxConstants.VideoContentType, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            type |= ChatMessageType.Videos;
        }

        if (SelectedChatFiles.FindIndex(f => f.ContentType.StartsWith(FluentCxConstants.ImageContentType, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            type |= ChatMessageType.Images;
        }

        if (SelectedChatFiles.FindIndex(f => !f.ContentType.StartsWith(FluentCxConstants.AudioContentType, StringComparison.OrdinalIgnoreCase)
            && !f.ContentType.StartsWith(FluentCxConstants.VideoContentType, StringComparison.OrdinalIgnoreCase)
            && !f.ContentType.StartsWith(FluentCxConstants.ImageContentType, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            type |= ChatMessageType.Files;
        }

        var message = new ChatMessage()
        {
            Id = -1,
            CreatedDate = DateTime.UtcNow,
            RoomId = roomId,
            SenderId = sender.Id,
            ReplyToMessageId = Reply?.Id,
            ReplyToMessage = Reply,
            Type = type,
            Sections = [.. texts.Select(t => new ChatMessageSection()
            {
                Id = -1,
                CreatedDate = DateTime.UtcNow,
                CultureName = t.Key,
                CultureId = sender.CultureId,
                MessageId = -1,
                Content = string.Join(Environment.NewLine, t.Value)
            })]
        };

        var files = new List<IBinaryChatFile>();

        foreach (var item in SelectedChatFiles)
        {
            var content = await item.GetDataAsync();

            var file = new BinaryChatFile()
            {
                Id = -1,
                MessageId = -1,
                CreatedDate = DateTime.UtcNow,
                Name = item.Name,
                Content = content,
                Length = content.Length,
                ContentType = item.ContentType
            };

            files.Add(file);
        }

        return new ChatMessageBuildResult([new(message, files)]);
    }
}
