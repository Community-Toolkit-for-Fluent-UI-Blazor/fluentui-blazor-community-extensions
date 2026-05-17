using System.Text.Json;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.Transport;

namespace FluentUI.Blazor.Community.Components.Chat.Engine;

internal sealed class ChatEngine
{
    private readonly IMessageTransport _transport;
    private  long _currentUserId;
    private readonly Dictionary<long, ChatRoom> _rooms = [];
    private readonly ChatRoomState _roomState;

    public ChatEngine(
        IMessageTransport transport,
        ChatRoomState roomState)
    {
        _transport = transport;
        _transport.RegisterMessageHandler(OnEnvelopeReceivedAsync);
        _roomState = roomState;
    }

    public void SetOwner(long userId)
    {
        _currentUserId = userId;
    }

    public async Task ConnectAsync() => await _transport.ConnectAsync();

    public async Task DisconnectAsync() => await _transport.DisconnectAsync();

    /// <summary>
    /// Send a message to all participants in the room that a new room has been created. This is typically called by the owner of the room after creating it.
    /// </summary>
    /// <param name="roomId">The ID of the newly created room.</param>
    /// <param name="ownerId">The ID of the owner of the newly created room.</param>
    /// <param name="payload">The payload containing additional information about the newly created room.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendCreatedRoomAsync(
        long roomId,
        long ownerId,
        JsonElement payload)
    {
        var envelope = new TransportEnvelope
        {
            Type = "room.created",
            RoomId = roomId,
            SenderId = ownerId,
            Payload = payload
        };

        await _transport.SendAsync(envelope);
    }

    public async Task SendMessageAsync(ChatRoom room, long messageId)
    {
        var payload = JsonSerializer.SerializeToElement(new
        {
            MessageId = messageId
        });

        var envelope = new TransportEnvelope
        {
            Type = "message.new",
            RoomId = room.Id,
            SenderId = _currentUserId,
            Payload = payload
        };

        await _transport.SendAsync(envelope);
    }

    private async Task OnEnvelopeReceivedAsync(TransportEnvelope envelope)
    {
        if (envelope.SenderId == _currentUserId)
        {
            return;
        }

        if (envelope.Type == "room.created")
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
            case "message.new":
                await HandleNewMessageAsync(room, envelope.Payload);
                break;

            case "room.updated":
                await HandleRoomUpdatedAsync(room, envelope.Payload);
                break;

            case "room.pinned":
                room.IsPinned = true;
                break;

            case "room.unpinned":
                room.IsPinned = false;
                break;

            case "room.muted":
                room.IsMuted = true;
                break;

            case "room.unmuted":
                room.IsMuted = false;
                break;

            case "room.hidden":
                room.IsHidden = true;
                break;

            case "room.unhidden":
                room.IsHidden = false;
                break;

            case "room.blocked":
                room.IsBlocked = true;
                break;

            case "room.unblocked":
                room.IsBlocked = false;
                break;

            case "room.archived":
                room.IsArchived = true;
                break;

            case "room.unarchived":
                room.IsArchived = false;
                break;

            default:
                break;
        }
    }

    private async Task HandleNewMessageAsync(ChatRoom room, JsonElement payload)
    {
        if (!payload.TryGetProperty("MessageId", out var idProp) ||
            !idProp.TryGetInt64(out var _))
        {
            return;
        }

      //  var message = await _roomState.GetMessageByIdAsync(messageId);
      //
      //  if (message is null)
      //  {
      //      return;
      //  }
      //
      //  room.LastMessage = message;
        room.IsEmpty = false;

        _roomState.UpdateRoom(room);
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
        await _transport.JoinRoomAsync(room.Id);
        _roomState.AddOrUpdateRoom(room);
    }

    private Task HandleRoomUpdatedAsync(ChatRoom room, JsonElement payload)
    {
        var updated = JsonSerializer.Deserialize<ChatRoom>(payload.GetRawText());

        if (updated is null)
        {
            return Task.CompletedTask;
        }

        room.Name = updated.Name;
        _roomState.UpdateRoom(room);

        return Task.CompletedTask;
    }

    public async Task RegisterRoomAsync(ChatRoom room)
    {
        _rooms[room.Id] = room;
        await _transport.JoinRoomAsync(room.Id);
    }

    public async Task UnegisterRoomAsync(ChatRoom room)
    {
        _rooms.Remove(room.Id);
        await _transport.LeaveRoomAsync(room.Id);
    }
}

