namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a delegate that defines the method signature for providing chat message items during the creation of chat messages.
/// </summary>
/// <param name="request">The request containing information needed to create chat message items.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains the created chat message.</returns>
public delegate ValueTask<ChatMessageCreationResult> ChatMessageItemsCreationProvider(ChatMessageItemsCreationRequest request);
