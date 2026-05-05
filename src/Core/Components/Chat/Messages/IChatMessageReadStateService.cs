using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a service that provides the read state of a chat message for a specific user.
/// </summary>
public interface IChatMessageReadStateService
{
    /// <summary>
    /// Retrieves the read state of a chat message for a specific user.
    /// </summary>
    /// <param name="message">The chat message for which to retrieve the read state.</param>
    /// <param name="currentUserId">The unique identifier of the user for whom to retrieve the read state.</param>
    /// <returns>Returns the read state of the chat message for the specified user.</returns>
    ChatMessageReadState GetReadState(IChatMessage message, long currentUserId);
}
