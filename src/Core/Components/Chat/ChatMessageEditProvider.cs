namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents a delegate that defines the method signature for providing chat message editing functionality.
/// </summary>
/// <param name="request">The request containing the details of the chat message to be edited.</param>
/// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
public delegate ValueTask ChatMessageEditProvider(ChatMessageEditRequest request);
