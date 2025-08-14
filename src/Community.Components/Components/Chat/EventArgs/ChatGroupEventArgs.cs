namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a chat group.
/// </summary>
/// <param name="users">Users to add to the chat.</param>
public record ChatGroupEventArgs(IEnumerable<ChatUser> users)
{
    /// <summary>
    /// Gets the users inside the chat.
    /// </summary>
    public IEnumerable<ChatUser> Users { get; } = users;

    /// <summary>
    /// Gets or sets the id of the chat group.
    /// </summary>
    public long ChatGroupIdReturnValue { get; set; }
}
