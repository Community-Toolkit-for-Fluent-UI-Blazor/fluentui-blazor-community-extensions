using System.Text.Json;

namespace FluentUI.Blazor.Community.Components.Chat.Transport;

/// <summary>
/// Represents a message envelope for transport between the chat engine and the client.
/// </summary>
public sealed class TransportEnvelope
{
    /// <summary>
    /// Gets or sets the type of the message, which indicates the kind of event or action being represented.
    /// </summary>
    /// <remarks>
    /// Some examples of message types include: "message.new" for a new message, "room.pinned" for a room being pinned, etc.
    /// </remarks>
    public string Type { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the room associated with this message.
    /// </summary>
    public long RoomId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the sender of the message. 
    /// </summary>
    public long SenderId { get; set; }

    /// <summary>
    /// Gets or sets the payload of the message, which contains the actual data relevant to the message type.
    /// </summary>
    public JsonElement Payload { get; set; }
}
