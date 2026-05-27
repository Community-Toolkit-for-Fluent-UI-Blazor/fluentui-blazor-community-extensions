using FluentUI.Blazor.Community.Components.Chat.Messages;

namespace FluentUI.Blazor.Community.Components.Chat.EventArgs;

/// <summary>
/// Represents the event arguments for when the last message in a chat room is updated.
/// </summary>
/// <param name="RoomId">The ID of the chat room.</param>
/// <param name="Message">The updated last message in the chat room.</param>
internal sealed record LastMessageUpdatedEventArgs(long RoomId, ChatMessage? Message);
