using System.Text.Json;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.SignalR;

namespace FluentUI.Blazor.Community.Components.Chat.Hubs;

/// <summary>
/// Represents a SignalR hub for managing chat communication between clients. This hub allows clients to join and leave chat rooms,
/// and facilitates the sending and receiving of messages within those rooms.
/// </summary>
public sealed class ChatHub : Hub
{
    /// <summary>
    /// Adds the current client to the group corresponding to the specified room, allowing them to receive messages sent to that room.
    /// </summary>
    public Task JoinRoomAsync(long roomId) => Groups.AddToGroupAsync(Context.ConnectionId, Invariant.ToString(roomId));

    /// <summary>
    /// Removes the current client from the group corresponding to the specified room.
    /// </summary>
    public Task LeaveRoomAsync(long roomId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, Invariant.ToString(roomId));

    /// <summary>
    /// Receives an envelope from a client and broadcasts it to other clients in the room.
    /// </summary>
    public async Task SendEnvelopeAsync(string type, long roomId, long senderId, JsonElement payload)
    {
        await Clients.OthersInGroup(Invariant.ToString(roomId)).SendAsync("ReceiveEnvelope", type, roomId, senderId, payload);
    }
}
