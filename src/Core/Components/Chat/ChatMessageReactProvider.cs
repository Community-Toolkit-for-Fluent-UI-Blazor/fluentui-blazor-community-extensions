namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a provider that handles reactions to chat messages.
/// </summary>
/// <param name="request">The request containing the chat message reaction details.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public delegate ValueTask ChatMessageReactProvider(ChatMessageReactRequest request);
