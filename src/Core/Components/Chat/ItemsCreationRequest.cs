using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a request to create chat message items in a room.
/// </summary>
/// <param name="RoomId">The identifier of the room where the items will be created.</param>
/// <param name="OwnerId">The identifier of the owner creating the items.</param>
/// <param name="Draft">The draft message to be created.</param>
/// <param name="SplitOption">The option specifying how to split the message.</param>
/// <param name="Token">The cancellation token to observe while waiting for the operation to complete.</param>
public sealed record ItemsCreationRequest(
    long RoomId,
    long OwnerId,
    ChatMessageDraft Draft,
    ChatMessageSplitOption SplitOption,
    CancellationToken Token);
