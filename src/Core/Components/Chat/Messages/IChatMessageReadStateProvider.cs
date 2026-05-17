using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a service that provides the read state of a chat message for a specific user.
/// </summary>
public interface IChatMessageReadStateProvider
{
    /// <summary>
    /// Retrieves the read state of a chat message for a specific user.
    /// </summary>
    /// <param name="messageId">The chat message for which to retrieve the read state.</param>
    /// <param name="userId">The unique identifier of the user for whom to retrieve the read state.</param>
    /// <returns>Returns the read state of the chat message for the specified user.</returns>
    Task<ChatMessageReadState> GetReadStateAsync(long messageId, long userId);
}
