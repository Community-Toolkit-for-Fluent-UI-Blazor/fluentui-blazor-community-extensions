using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to edit a chat message.
/// </summary>
/// <param name="Message">The chat message to be edited.</param>
/// <param name="token">The token to monitor for cancellation requests.</param>
public sealed record ChatMessageEditRequest(
    ChatMessage Message,
    CancellationToken token);
