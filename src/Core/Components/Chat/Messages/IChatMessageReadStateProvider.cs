namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a service that provides the read state of chat messages.
/// </summary>
public interface ChatMessageReadStateProvider
{
    /// <summary>
    /// Retrieves the read state of the specified chat messages.
    /// </summary>
    /// <param name="messageId">The unique identifiers of the chat messages.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only dictionary mapping message IDs to their read states.</returns>
    Task<IReadOnlyDictionary<long, IReadOnlyList<ChatMessageUserState>>> GetReadStateAsync(params long[] messageId);
}
