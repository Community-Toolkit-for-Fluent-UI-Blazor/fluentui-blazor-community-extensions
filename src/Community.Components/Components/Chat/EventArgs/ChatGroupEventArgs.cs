namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a chat group.
/// </summary>
/// <param name="Users">Users to add to the chat.</param>
public record ChatGroupEventArgs(IEnumerable<ChatUser> Users)
{
    /// <summary>
    /// Gets or sets the id of the chat group.
    /// </summary>
    public long ChatGroupIdReturnValue { get; set; }
}
