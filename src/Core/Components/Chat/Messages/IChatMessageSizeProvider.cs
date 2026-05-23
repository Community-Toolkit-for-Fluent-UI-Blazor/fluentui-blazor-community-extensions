using FluentUI.Blazor.Community.Components.Base;

namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a service that provides the size of a chat message.
/// </summary>
public interface IChatMessageSizeProvider
{
    /// <summary>
    /// Asynchronously retrieves the size associated with the specified message.
    /// </summary>
    /// <param name="messageId">The unique identifier of the message.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only dictionary mapping message IDs to their sizes.</returns>
    ValueTask<IReadOnlyDictionary<long, SizeD>> GetSizeAsync(params long[] messageId);
}
