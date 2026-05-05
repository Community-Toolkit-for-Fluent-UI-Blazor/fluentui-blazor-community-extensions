namespace FluentUI.Blazor.Community.Components.Chat;

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
    /// Gets or sets the name of the chat group (optional).
    /// </summary>
    public string? GroupName { get; init; }

    /// <summary>
    /// Gets or sets the description of the chat group (optional).
    /// </summary>
    public string? Description { get; init; }
}
