using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// A delegate that provides functionality to delete a chat message asynchronously.
/// </summary>
/// <param name="message">The chat message to delete.</param>
/// <param name="token">The cancellation token to observe while waiting for the operation to complete.</param>
/// <returns>A task that represents the asynchronous delete operation.</returns>
public delegate ValueTask ChatMessageDeleteMessageProvider(ChatMessage message, CancellationToken token);
