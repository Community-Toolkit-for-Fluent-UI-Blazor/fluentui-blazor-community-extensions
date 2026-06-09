using System.Text.Json;
using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.Transport;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

internal sealed class ChatEngine
{
    private readonly IMessageTransport _transport;
    private readonly ChatRoomViewState _roomState;
    private readonly ChatMessageState _messageState;
    private readonly ChatMessageDynamicState _dynamicState;
    private readonly Dictionary<long, ChatRoomView> _rooms = [];
    private long _currentUserId;
    private ChatMessageItemCollectionProvider? _getMessagesByIds;
    private ChatMessageUserStateProvider? _getReadStates;
    private ChatMessageFileCollectionProvider? _getFiles;
    private ChatMessageReactionCollectionProvider? _getReactions;

    public ChatEngine(
        IMessageTransport transport,
        ChatRoomViewState roomState,
        ChatMessageState messageState,
        ChatMessageDynamicState dynamicState)
    {
        _transport = transport;
        _roomState = roomState;
        _messageState = messageState;
        _dynamicState = dynamicState;

        _transport.RegisterMessageHandler(OnEnvelopeReceivedAsync);
    }

    public ChatEngine SetFilesProvider(
        ChatMessageFileCollectionProvider? provider,
        bool condition)
    {
        if (condition)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _getFiles = provider;
        }

        return this;
    }

    public ChatEngine SetReactionsProvider(
        ChatMessageReactionCollectionProvider? provider,
        bool condition)
    {
        if (condition)
        {
            ArgumentNullException.ThrowIfNull(provider);
            _getReactions = provider;
        }

        return this;
    }

    public ChatEngine SetMessageItemProvider(ChatMessageItemCollectionProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _getMessagesByIds = provider;

        return this;
    }

    public ChatEngine SetUserStateProvider(ChatMessageUserStateProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _getReadStates = provider;

        return this;
    }

    public void SetOwner(long userId) => _currentUserId = userId;

    public Task ConnectAsync() => _transport.ConnectAsync();

    public Task DisconnectAsync() => _transport.DisconnectAsync();

    private Task SendSimpleEventAsync(string type, long roomId, long messageId, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new { MessageId = messageId });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = type,
            RoomId = roomId,
            SenderId = _currentUserId,
            Payload = payload,
        }, cancellationToken);
    }

    public Task SendNewMessageAsync(ChatRoomView room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageNew, room.Room.Id, messageId, cancellationToken);

    public Task SendNewMessagesAsync(ChatRoomView room, IEnumerable<long> messageIds, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageIds = messageIds.ToArray()
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessagesNew,
            RoomId = room.Room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public Task SendEditedMessageAsync(ChatRoomView room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageEdit, room.Room.Id, messageId, cancellationToken);

    public Task SendDeletedMessageAsync(ChatRoomView room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageDelete, room.Room.Id, messageId, cancellationToken);

    public Task SendMessageReadAsync(ChatRoomView room, long messageId, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageId = messageId
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessageRead,
            RoomId = room.Room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public async Task RegisterRoomAsync(ChatRoomView room, CancellationToken cancellationToken)
    {
        _rooms[room.Room.Id] = room;
        await _transport.JoinRoomAsync(room.Room.Id, cancellationToken);
    }

    public async Task UnregisterRoomAsync(ChatRoomView room, CancellationToken cancellationToken)
    {
        _rooms.Remove(room.Room.Id);
        await _transport.LeaveRoomAsync(room.Room.Id, cancellationToken);
    }

    public Task SendReactedMessageAsync(
        ChatRoomView roomView,
        long messageId,
        string reaction,
        CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageId = messageId,
            Reaction = reaction,
            UserId = _currentUserId
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessageReact,
            RoomId = roomView.Room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public Task SendCreatedRoomAsync(ChatRoomView roomView, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            RoomId = roomView.Room.Id
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.RoomCreated,
            RoomId = roomView.Room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    private async Task OnEnvelopeReceivedAsync(TransportEnvelope envelope)
    {
        if (envelope.SenderId == _currentUserId)
        {
            return;
        }

        if (envelope.Type == ChatMessageTypes.RoomCreated)
        {
            await HandleRoomCreatedAsync(envelope.Payload);
            return;
        }

        if (!_rooms.TryGetValue(envelope.RoomId, out var roomView))
        {
            return;
        }

        switch (envelope.Type)
        {
            case ChatMessageTypes.MessagesNew:
                await HandleNewMessagesAsync(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.MessageNew:
                await HandleNewMessageAsync(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.MessageEdit:
                await HandleEditedMessageAsync(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.MessageDelete:
                HandleDeletedMessage(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.MessageRead:
                await HandleMessageReadAsync(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.MessageReact:
                HandleMessageReacted(roomView, envelope.Payload);
                break;

            case ChatMessageTypes.RoomUpdated:
                HandleRoomUpdated(roomView, envelope.Payload);
                break;
        }
    }

    private void HandleMessageReacted(ChatRoomView roomView, JsonElement payload)
    {
        if (!payload.TryGetProperty("MessageId", out var idProp) ||
            !idProp.TryGetInt64(out var messageId))
        {
            return;
        }

        if (!payload.TryGetProperty("Reaction", out var reactionProp) ||
            reactionProp.GetString() is not string reaction)
        {
            return;
        }

        if (!payload.TryGetProperty("UserId", out var userProp) ||
            !userProp.TryGetInt64(out var userId))
        {
            return;
        }

        var list = _dynamicState.GetReactions(roomView.Room, messageId)?.ToList() ?? [];
        list.Add(new ChatMessageReaction
        {
            MessageId = messageId,
            UserReactedById = userId,
            Emoji = reaction
        });

        _dynamicState.SetReactions(roomView.Room, messageId, list);
    }

    private async Task HandleRoomCreatedAsync(JsonElement payload)
    {
        if (!payload.TryGetProperty("RoomId", out var idProp) ||
            !idProp.TryGetInt64(out var roomId))
        {
            return;
        }

        var room = _roomState.GetRoom(roomId);

        if (room is null)
        {
            return;
        }

        _rooms[room.Room.Id] = room;
        using var cts = new CancellationTokenSource();
        await _transport.JoinRoomAsync(room.Room.Id, cts.Token);
        _roomState.AddOrUpdateRoom(room);
    }

    private async Task HandleMessagesAsync(ChatRoomView roomView, long[] messageIds)
    {
        if (messageIds.Length == 0)
        {
            return;
        }

        if (_getMessagesByIds is null ||
            _getReadStates is null)
        {
            return;
        }

        var filesDict = new Dictionary<long, List<IChatFile>>();
        var reactionsDict = new Dictionary<long, List<ChatMessageReaction>>();
        var readStatesDict = new Dictionary<long, List<ChatMessageUserState>>();

        using var cts = new CancellationTokenSource();
        var messages = await _getMessagesByIds(new(roomView.Room.Id, messageIds, cts.Token));

        if (_getReadStates is not null)
        {
            var result = await _getReadStates(new(messageIds, cts.Token));

            foreach (var item in result)
            {
                if (!readStatesDict.TryGetValue(item.MessageId, out var list))
                {
                    list = [];
                    readStatesDict[item.MessageId] = list;
                }

                readStatesDict[item.MessageId].Add(item);
            }
        }

        if (_getFiles is not null)
        {
            var result = await _getFiles(new(messageIds, cts.Token));

            foreach (var item in result)
            {
                if (!filesDict.TryGetValue(item.MessageId, out var list))
                {
                    list = [];
                    filesDict[item.MessageId] = list;
                }

                filesDict[item.MessageId].Add(item);
            }
        }

        if (_getReactions is not null)
        {
            var result = await _getReactions(new(messageIds, cts.Token));

            foreach (var item in result)
            {
                if (!reactionsDict.TryGetValue(item.MessageId, out var list))
                {
                    list = [];
                    reactionsDict[item.MessageId] = list;
                }

                reactionsDict[item.MessageId].Add(item);
            }
        }

        for (var i = 0; i < messages.Count; i++)
        {
            var message = messages[i];

            if (message is null)
            {
                continue;
            }

            var id = messageIds[i];

            _messageState.AddOrUpdateMessage(roomView.Room, message);

            var userStates = readStatesDict.TryGetValue(id, out var s) ? s : [];
            var users = roomView.Users;
            var readState = ChatMessageReadStateCalculator.Compute(
                _currentUserId,
                userStates,
                users);

            _dynamicState.SetReadState(roomView.Room, id, readState);

            if (filesDict.TryGetValue(id, out var files))
            {
                _dynamicState.SetFiles(roomView.Room, id, files);
            }

            if (reactionsDict.TryGetValue(id, out var reactions))
            {
                _dynamicState.SetReactions(roomView.Room, id, reactions);
            }
        }
    }

    private async Task HandleNewMessagesAsync(ChatRoomView roomView, JsonElement payload)
    {
        if (!payload.TryGetProperty("MessageIds", out var idsProp) ||
            idsProp.ValueKind != JsonValueKind.Array)
        {
            return;
        }

        var messageIds = idsProp.EnumerateArray()
            .Where(x => x.TryGetInt64(out _))
            .Select(x => x.GetInt64())
            .ToArray();

        await HandleMessagesAsync(roomView, messageIds);
    }

    private async Task HandleNewMessageAsync(ChatRoomView roomView, JsonElement payload)
    {
        if (!TryGetMessageId(payload, out var messageId))
        {
            return;
        }

        await HandleMessagesAsync(roomView, [messageId]);
    }

    private async Task HandleEditedMessageAsync(ChatRoomView roomView, JsonElement payload)
    {
        if (!TryGetMessageId(payload, out var messageId))
        {
            return;
        }

        if (_getMessagesByIds is null ||
            _getReadStates is null)
        {
            return;
        }

        using var cts = new CancellationTokenSource();

        var message = await _getMessagesByIds(new(roomView.Room.Id, [messageId], cts.Token));

        if (message is null ||
            message.Count == 0)
        {
            return;
        }

        var readStatesDict = new Dictionary<long, List<ChatMessageUserState>>();

        if (_getReadStates is not null)
        {
            var result = await _getReadStates(new([messageId], cts.Token));

            foreach (var item in result)
            {
                if (!readStatesDict.TryGetValue(item.MessageId, out var list))
                {
                    list = [];
                    readStatesDict[item.MessageId] = list;
                }

                readStatesDict[item.MessageId].Add(item);
            }
        }

        var userStates = readStatesDict.TryGetValue(messageId, out var s) ? s : [];
        var users = roomView.Users;

        var readState = ChatMessageReadStateCalculator.Compute(
            _currentUserId,
            userStates,
            users);

        _messageState.AddOrUpdateMessage(roomView.Room, message[0]);
        _dynamicState.SetReadState(roomView.Room, messageId, readState);
    }

    private void HandleDeletedMessage(ChatRoomView roomView, JsonElement payload)
    {
        if (!TryGetMessageId(payload, out var messageId))
        {
            return;
        }

        _messageState.RemoveMessage(roomView.Room, messageId);
        _dynamicState.Clear(roomView.Room, messageId);
    }

    private async Task HandleMessageReadAsync(ChatRoomView roomView, JsonElement payload)
    {
        if (_getReadStates is null)
        {
            return;
        }

        if (!payload.TryGetProperty("MessageId", out var idProp) ||
            !idProp.TryGetInt64(out var messageId))
        {
            return;
        }

        using var cts = new CancellationTokenSource();
        var readStatesDict = new Dictionary<long, List<ChatMessageUserState>>();

        if (_getReadStates is not null)
        {
            var result = await _getReadStates(new([messageId], cts.Token));

            foreach (var item in result)
            {
                if (!readStatesDict.TryGetValue(item.MessageId, out var list))
                {
                    list = [];
                    readStatesDict[item.MessageId] = list;
                }

                readStatesDict[item.MessageId].Add(item);
            }
        }

        var userStates = readStatesDict.TryGetValue(messageId, out var s) ? s : [];
        var users = roomView.Users;
        var readState = ChatMessageReadStateCalculator.Compute(
            _currentUserId,
            userStates,
            users);

        _dynamicState.SetReadState(roomView.Room, messageId, readState);
    }

    private void HandleRoomUpdated(ChatRoomView roomView, JsonElement payload)
    {
        var updated = JsonSerializer.Deserialize<ChatRoom>(payload.GetRawText());

        if (updated is null)
        {
            return;
        }

        if (roomView.Room.Id == updated.Id)
        {
            roomView = roomView with
            {
                Room = roomView.Room with
                {
                    Name = updated.Name,
                    IsLocked = updated.IsLocked,
                    IsDeleted = updated.IsDeleted
                }
            };

            _roomState.UpdateRoom(roomView);
        }
    }

    private static bool TryGetMessageId(JsonElement payload, out long messageId)
    {
        messageId = default;

        if (!payload.TryGetProperty("MessageId", out var idProp))
        {
            return false;
        }

        return idProp.TryGetInt64(out messageId);
    }

    internal Task SendUpdatedRoomAsync(ChatRoom? room, CancellationToken cancellationToken = default)
    {
        if (room is null)
        {
            return Task.CompletedTask;
        }

        var payload = JsonSerializer.SerializeToElement(new
        {
            RoomId = room.Id,
            room.Name
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.RoomUpdated,
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }
}
