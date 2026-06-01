using System.Text.Json;
using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.Transport;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

internal sealed class ChatEngine
{
    private readonly IMessageTransport _transport;
    private readonly ChatRoomState _roomState;
    private readonly ChatMessageState _messageState;
    private readonly ChatMessageDynamicState _dynamicState;
    private readonly ChatRoomDynamicState _roomDynamicState;
    private readonly Dictionary<long, ChatRoom> _rooms = [];
    private long _currentUserId;
    private ChatMessageItemCollectionProvider? _getMessagesByIds;
    private ChatMessageUserStateProvider? _getReadStates;
    private ChatMessageFileCollectionProvider? _getFiles;
    private ChatMessageReactionCollectionProvider? _getReactions;
    private ChatRoomUsersProvider? _getUsers;
    private ChatLastMessageProvider? _getLastMessage;
    private ChatUnreadMessagesProvider? _getUnread;

    public ChatEngine(
        IMessageTransport transport,
        ChatRoomState roomState,
        ChatMessageState messageState,
        ChatMessageDynamicState dynamicState,
        ChatRoomDynamicState roomDynamicState)
    {
        _transport = transport;
        _roomState = roomState;
        _messageState = messageState;
        _dynamicState = dynamicState;
        _roomDynamicState = roomDynamicState;

        _transport.RegisterMessageHandler(OnEnvelopeReceivedAsync);
    }

    private async Task LoadRoomDynamicDataAsync(long roomId)
    {
        using var cts = new CancellationTokenSource();

        if (_getUsers is not null)
        {
            var users = await _getUsers(roomId, cts.Token);
            _roomDynamicState.SetUsers(roomId, users);
        }

        if (_getLastMessage is not null)
        {
            var last = await _getLastMessage(roomId, cts.Token);
            _roomDynamicState.SetLastMessage(roomId, last);
        }

        if (_getUnread is not null)
        {
            var unread = await _getUnread(roomId, _currentUserId, cts.Token);
            _roomDynamicState.SetUnreadCount(roomId, unread);
        }
    }

    public ChatEngine SetUsersProvider(ChatRoomUsersProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _getUsers = provider;

        return this;
    }

    public ChatEngine SetLastMessageProvider(ChatLastMessageProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _getLastMessage = provider;

        return this;
    }

    public ChatEngine SetUnreadProvider(ChatUnreadMessagesProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _getUnread = provider;

        return this;
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

    public Task SendNewMessageAsync(ChatRoom room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageNew, room.Id, messageId, cancellationToken);

    public Task SendNewMessagesAsync(ChatRoom room, IEnumerable<long> messageIds, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageIds = messageIds.ToArray()
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessagesNew,
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public Task SendEditedMessageAsync(ChatRoom room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageEdit, room.Id, messageId, cancellationToken);

    public Task SendDeletedMessageAsync(ChatRoom room, long messageId, CancellationToken cancellationToken)
        => SendSimpleEventAsync(ChatMessageTypes.MessageDelete, room.Id, messageId, cancellationToken);

    public Task SendMessageReadAsync(ChatRoom room, long messageId, IEnumerable<long> readers, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageId = messageId,
            Readers = readers.ToArray()
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessageRead,
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public async Task RegisterRoomAsync(ChatRoom room, CancellationToken cancellationToken)
    {
        _rooms[room.Id] = room;
        await _transport.JoinRoomAsync(room.Id, cancellationToken);
        await LoadRoomDynamicDataAsync(room.Id);
    }

    public async Task UnregisterRoomAsync(ChatRoom room, CancellationToken cancellationToken)
    {
        _rooms.Remove(room.Id);
        await _transport.LeaveRoomAsync(room.Id, cancellationToken);
    }

    public Task SendReactedMessageAsync(ChatRoom room, long messageId, string reaction, CancellationToken cancellationToken)
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
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public Task SendMessageReadAsync(ChatRoom room, long messageId, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageId = messageId,
            UserId = _currentUserId
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.MessageRead,
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        }, cancellationToken);
    }

    public Task SendCreatedRoomAsync(ChatRoom room, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            RoomId = room.Id
        });

        return _transport.SendAsync(new TransportEnvelope
        {
            Type = ChatMessageTypes.RoomCreated,
            RoomId = room.Id,
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

        if (!_rooms.TryGetValue(envelope.RoomId, out var room))
        {
            return;
        }

        switch (envelope.Type)
        {
            case ChatMessageTypes.MessagesNew:
                await HandleNewMessagesAsync(room, envelope.Payload);
                break;

            case ChatMessageTypes.MessageNew:
                await HandleNewMessageAsync(room, envelope.Payload);
                break;

            case ChatMessageTypes.MessageEdit:
                await HandleEditedMessageAsync(room, envelope.Payload);
                break;

            case ChatMessageTypes.MessageDelete:
                HandleDeletedMessage(room, envelope.Payload);
                break;

            case ChatMessageTypes.MessageRead:
                await HandleMessageReadAsync(room, envelope.Payload);
                break;

            case ChatMessageTypes.MessageReact:
                HandleMessageReacted(room, envelope.Payload);
                break;

            case ChatMessageTypes.RoomUpdated:
                HandleRoomUpdated(room, envelope.Payload);
                break;
        }
    }

    private void HandleMessageReacted(ChatRoom room, JsonElement payload)
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

        var list = _dynamicState.GetReactions(room, messageId)?.ToList() ?? [];
        list.Add(new ChatMessageReaction
        {
            MessageId = messageId,
            UserReactedById = userId,
            Emoji = reaction
        });

        _dynamicState.SetReactions(room, messageId, list);
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

        _rooms[room.Id] = room;
        using var cts = new CancellationTokenSource();
        await _transport.JoinRoomAsync(room.Id, cts.Token);
        _roomState.AddOrUpdateRoom(room);

        await LoadRoomDynamicDataAsync(room.Id);
    }

    private async Task HandleMessagesAsync(ChatRoom room, long[] messageIds)
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
        var messages = await _getMessagesByIds(new(room.Id, messageIds, cts.Token));

        if (_getReadStates is not null)
        {
            var result = await _getReadStates(new(room.Id, messageIds, cts.Token));

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

            _messageState.AddOrUpdateMessage(room, message);

            var userStates = readStatesDict.TryGetValue(id, out var s) ? s : [];
            var users = _roomDynamicState.GetUsers(room.Id);
            var readState = ChatMessageReadStateCalculator.Compute(
                _currentUserId,
                userStates,
                users);

            _dynamicState.SetReadState(room, id, readState);

            if (filesDict.TryGetValue(id, out var files))
            {
                _dynamicState.SetFiles(room, id, files);
            }

            if (reactionsDict.TryGetValue(id, out var reactions))
            {
                _dynamicState.SetReactions(room, id, reactions);
            }

            var lastMessage = _roomDynamicState.GetLastMessage(room.Id);

            if (lastMessage is null ||
                lastMessage.CreatedDate < message.CreatedDate)
            {
                _roomDynamicState.SetLastMessage(room.Id, message);
                room.IsEmpty = false;
                _roomState.UpdateRoom(room);
            }
        }
    }

    private async Task HandleNewMessagesAsync(ChatRoom room, JsonElement payload)
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

        await HandleMessagesAsync(room, messageIds);
    }

    private async Task HandleNewMessageAsync(ChatRoom room, JsonElement payload)
    {
        if (!TryGetMessageId(payload, out var messageId))
        {
            return;
        }

        await HandleMessagesAsync(room, [messageId]);
    }

    private async Task HandleEditedMessageAsync(ChatRoom room, JsonElement payload)
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

        var message = await _getMessagesByIds(new(room.Id, [messageId], cts.Token));

        if (message is null ||
            message.Count == 0)
        {
            return;
        }

        var readStatesDict = new Dictionary<long, List<ChatMessageUserState>>();

        if (_getReadStates is not null)
        {
            var result = await _getReadStates(new(room.Id, [messageId], cts.Token));

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
        var users = _roomDynamicState.GetUsers(room.Id);

        var readState = ChatMessageReadStateCalculator.Compute(
            _currentUserId,
            userStates,
            users);

        _messageState.AddOrUpdateMessage(room, message[0]);
        _dynamicState.SetReadState(room, messageId, readState);

        var lastMessage = _roomDynamicState.GetLastMessage(room.Id);

        if (lastMessage is null ||
            lastMessage.CreatedDate < message[0].CreatedDate)
        {
            _roomDynamicState.SetLastMessage(room.Id, message[0]);
            room.IsEmpty = false;
            _roomState.UpdateRoom(room);
        }
    }

    private void HandleDeletedMessage(ChatRoom room, JsonElement payload)
    {
        if (!TryGetMessageId(payload, out var messageId))
        {
            return;
        }

        _messageState.RemoveMessage(room, messageId);
        _dynamicState.Clear(room, messageId);

        var last = _messageState.GetLastMessage(room.Id);
        _roomDynamicState.SetLastMessage(room.Id, last);

        var unread = ChatUnreadCalculator.Compute(
            room,
            _currentUserId,
            _messageState,
            _dynamicState
        );

        _roomDynamicState.SetUnreadCount(room.Id, unread);
        _roomState.UpdateRoom(room);
    }

    private async Task HandleMessageReadAsync(ChatRoom room, JsonElement payload)
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
            var result = await _getReadStates(new(room.Id, [messageId], cts.Token));

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
        var users = _roomDynamicState.GetUsers(room.Id);
        var readState = ChatMessageReadStateCalculator.Compute(
            _currentUserId,
            userStates,
            users);

        _dynamicState.SetReadState(room, messageId, readState);
    }

    private void HandleRoomUpdated(ChatRoom room, JsonElement payload)
    {
        var updated = JsonSerializer.Deserialize<ChatRoom>(payload.GetRawText());

        if (updated is null)
        {
            return;
        }

        if (room.Id == updated.Id)
        {
            room.Name = updated.Name;
            _roomState.UpdateRoom(room);
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
