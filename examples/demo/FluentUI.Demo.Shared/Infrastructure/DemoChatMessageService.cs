using System.Globalization;
using System.Security.Cryptography;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Services;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class DemoChatMessageService(ChatState chatState)
    : IChatMessageService
{
    private readonly Dictionary<long, List<ChatMessage>> _messages = [];
    private readonly Dictionary<string, long> _cultures = new()
    {
        ["en-US"] = 1,
        ["fr-FR"] = 2,
        ["de-DE"] = 3,
        ["es-ES"] = 4,
        ["zh-CN"] = 5
    };

    public ValueTask AddReactionAsync(ChatMessageReactRequest value)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IEnumerable<IChatMessage>> CreateMessagesAsync(ChatMessageCreationRequest value)
    {
        return value.SplitOption switch
        {
            ChatMessageSplitOption.Split => CreateSplitMessagesAsync(value),
            ChatMessageSplitOption.None => CreateMessageAsync(value),
            _ => throw new NotImplementedException($"Unsupported split option: {value.SplitOption}")
        };
    }

    private async ValueTask<IEnumerable<IChatMessage>> CreateMessageAsync(ChatMessageCreationRequest value)
    {
        // If the chat room is loaded from a database, this line isn't useful.
        if (chatState.Room?.IsEmpty ?? false)
        {
            chatState.Room.IsEmpty = false;
        }

        if (!_messages.ContainsKey(value.RoomId))
        {
            _messages.Add(value.RoomId, []);
        }

        if (_messages.TryGetValue(value.RoomId, out var messages))
        {
            var newMessage = new ChatMessage
            {
                Id = messages.Count + 1,
                CreatedDate = DateTime.UtcNow,
                Edited = false,
                IsDeleted = false,
                IsPinned = false,
                Sender = value.Owner,
                ReplyMessageId = value.ChatDraft.Reply?.Id,
                ReplyMessage = value.ChatDraft.Reply
            };

            if (value.ChatDraft.SelectedChatFiles.Count > 0)
            {
                newMessage.MessageType |= ChatMessageType.Document;

                foreach (var item in value.ChatDraft.SelectedChatFiles.OrderBy(x => x.Id))
                {
                    var id = item.Id ?? $"f{newMessage.Files.Count + 1}";

                    if (!int.TryParse(id, CultureInfo.InvariantCulture, out var fileId))
                    {
                        fileId = RandomNumberGenerator.GetInt32(int.MaxValue);
                    }

                    newMessage.Files.Add(new BinaryChatFile
                    {
                        Id = fileId,
                        Name = item.Name,
                        ContentType = item.ContentType,
                        Data = item.DataFunc != null ? await item.DataFunc() : item.Data
                    });
                }
            }

            if (!string.IsNullOrEmpty(value.ChatDraft.Text))
            {
                newMessage.MessageType |= ChatMessageType.Text;

                newMessage.Sections.Add(new ChatMessageSection
                {
                    Id = 1,
                    Content = value.ChatDraft.Text,
                    CreatedDate = DateTime.UtcNow,
                    CultureId = value.Owner.CultureId,
                    MessageId = newMessage.Id
                });
            }

            if (value.IsTranslationEnabled)
            {
                foreach (var item in value.ChatDraft.GetTranslatedTexts())
                {
                    foreach (var subItem in item.Value)
                    {
                        newMessage.Sections.Add(new ChatMessageSection
                        {
                            Id = newMessage.Sections.Count + 1,
                            Content = subItem,
                            CreatedDate = DateTime.UtcNow,
                            CultureId = _cultures[item.Key],
                            MessageId = newMessage.Id
                        });
                    }
                }
            }

            messages.Add(newMessage);
            await Task.Delay(1);

            return [newMessage];
        }

        return [];
    }

    private async ValueTask<IEnumerable<IChatMessage>> CreateSplitMessagesAsync(ChatMessageCreationRequest value)
    {
        // If the chat room is loaded from a database, this line isn't useful.
        if (chatState.Room?.IsEmpty ?? false)
        {
            chatState.Room.IsEmpty = false;
        }

        if (!_messages.ContainsKey(value.RoomId))
        {
            _messages.Add(value.RoomId, []);
        }

        if (_messages.TryGetValue(value.RoomId, out var messages))
        {
            List<IChatMessage> splitMessages = [];

            if (!string.IsNullOrEmpty(value.ChatDraft.Text))
            {
                var newMessage = new ChatMessage
                {
                    Id = messages.Count + 1,
                    CreatedDate = DateTime.UtcNow,
                    Edited = false,
                    IsDeleted = false,
                    IsPinned = false,
                    Sender = value.Owner,
                    MessageType = ChatMessageType.Text
                };

                newMessage.Sections.Add(new ChatMessageSection
                {
                    Id = 1,
                    Content = value.ChatDraft.Text,
                    CreatedDate = DateTime.UtcNow,
                    CultureId = value.Owner.CultureId,
                    MessageId = newMessage.Id
                });

                messages.Add(newMessage);
                splitMessages.Add(newMessage);
            }

            if (value.ChatDraft.SelectedChatFiles.Count > 0)
            {
                var newMessage = new ChatMessage
                {
                    Id = messages.Count + 1,
                    CreatedDate = DateTime.UtcNow,
                    Edited = false,
                    IsDeleted = false,
                    IsPinned = false,
                    Sender = value.Owner,
                    MessageType = ChatMessageType.Document
                };

                foreach (var item in value.ChatDraft.SelectedChatFiles.OrderBy(x => x.Id))
                {
                    var id = item.Id ?? $"f{newMessage.Files.Count + 1}";

                    if (!int.TryParse(id.AsSpan(1), CultureInfo.InvariantCulture, out var idResult))
                    {
                        idResult = RandomNumberGenerator.GetInt32(int.MaxValue);
                    }

                    newMessage.Files.Add(new BinaryChatFile
                    {
                        Id = idResult,
                        Name = item.Name,
                        ContentType = item.ContentType,
                        Data = item.DataFunc != null ? await item.DataFunc() : item.Data
                    });
                }

                messages.Add(newMessage);
                splitMessages.Add(newMessage);
            }

            return splitMessages;
        }

        return [];
    }

    public ValueTask DeleteAsync(long id, IChatMessage message)
    {
        var msg = _messages.GetValueOrDefault(id)?.Find(x => x.Id == message.Id);

        if (msg is not null)
        {
            msg.IsDeleted = true;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask EditMessageAsync(ChatMessageEditRequest value)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<IChatMessage?> GetMessageAsync(long roomId, long messageId)
    {
        var message = _messages
            .GetValueOrDefault(roomId)?
            .FirstOrDefault(m => m.Id == messageId);

        await Task.Delay(1);

        return message;
    }

    public ValueTask<IEnumerable<IChatMessage>> GetMessageListAsync(ChatMessageItemRequest value)
    {
        if (_messages.TryGetValue(value.RoomId, out var messages))
        {
            return ValueTask.FromResult<IEnumerable<IChatMessage>>(messages);
        }

        return ValueTask.FromResult<IEnumerable<IChatMessage>>([]);
    }

    public ValueTask<int> MessageCountAsync(ChatMessageCountRequest value)
    {
        var count = _messages.TryGetValue(value.RoomId, out var messages) ? messages.Count : 0;

        return ValueTask.FromResult(count);
    }

    public ValueTask PinOrUnpinAsync(PinOrUnpinRequest pinOrUnpinRequest)
    {
        var message = _messages.GetValueOrDefault(pinOrUnpinRequest.RoomId)?.FirstOrDefault(x => x.Id == pinOrUnpinRequest.Message.Id);

        if (message != null)
        {
            message.IsPinned = pinOrUnpinRequest.Pin;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask SetReadStateAsync(long roomId, long id, ChatUser owner, bool read)
    {
        var message = _messages.GetValueOrDefault(roomId)?.FirstOrDefault(x => x.Id == id);

        message?.SetReadState(owner, read);

        return ValueTask.CompletedTask;
    }
}
