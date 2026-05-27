namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents a request to create a chat group with selected users.
/// </summary>
public sealed record ChatGroupCreateRequest
{
    /// <summary>
    /// Gets or sets the list of users to be added to the chat group.
    /// </summary>
    public required IEnumerable<ChatUser> Users { get; init; }

    /// <summary>
    /// Gets the created room.
    /// </summary>
    public required ChatRoom Room { get; init; }
}
