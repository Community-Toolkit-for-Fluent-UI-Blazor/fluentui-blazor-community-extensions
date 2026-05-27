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
    /// <param name="roomId">The identifier of the chat room to join.</param>
    public async Task JoinRoomAsync(long roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Invariant.ToString(roomId));
    }
    /// <summary>
    /// Removes the current client from the group corresponding to the specified room.
    /// </summary>
    /// <param name="roomId">The identifier of the chat room to leave.</param>
    public async Task LeaveRoomAsync(long roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, Invariant.ToString(roomId));
    }

    /// <summary>
    /// Receives an envelope from a client and broadcasts it to other clients in the room.
    /// </summary>
    /// <param name="type">The type of the envelope being sent.</param>
    /// <param name="roomId">The identifier of the chat room to which the message belongs.</param>
    /// <param name="senderId">The identifier of the client sending the message.</param>
    /// <param name="payload">The content of the message being sent, represented as a JSON element.</param>
    public async Task SendEnvelopeAsync(
        string type,
        long roomId,
        long senderId,
        JsonElement payload)
    {
        await Clients.OthersInGroup(Invariant.ToString(roomId)).SendAsync("ReceiveEnvelope", type, roomId, senderId, payload);
    }
}
