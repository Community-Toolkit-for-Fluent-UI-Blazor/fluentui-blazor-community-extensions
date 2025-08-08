
namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the constants to use for the <see cref="FluentUI.Blazor.Community.Services.IMessageService"/>
/// </summary>
public static class ChatMessageListViewConstants
{
    /// <summary>
    /// Gets the method name for receiving a message.
    /// </summary>
    public const string ReceiveMessages = nameof(ReceiveMessages);

    /// <summary>
    /// Gets the method name when a message was deleted.
    /// </summary>
    public const string MessageDeleted = nameof(MessageDeleted);

    /// <summary>
    /// Gets the method name when the react was treated.
    /// </summary>
    public const string ReactOnMessage = nameof(ReactOnMessage);

    /// <summary>
    /// Gets the method name when the message is pined or unpined.
    /// </summary>
    public const string PinOrUnpin = nameof(PinOrUnpin);

    /// <summary>
    /// Gets the method name when a user reacts on a message.
    /// </summary>
    public const string SendReactOnMessageAsync = nameof(SendReactOnMessageAsync);

    /// <summary>
    /// Gets the method name to pin or unpin the message.
    /// </summary>
    public const string PinOrUnpinAsync = nameof(PinOrUnpinAsync);

    /// <summary>
    /// Gets the method name to delete the message.
    /// </summary>
    public const string DeleteMessageAsync = nameof(DeleteMessageAsync);

    /// <summary>
    /// Gets the method name to mark the message as read.
    /// </summary>
    public const string MessageReadAsync = nameof(MessageReadAsync);

    /// <summary>
    /// Gets the method name to send the messages.
    /// </summary>
    public const string SendMessagesAsync = nameof(SendMessagesAsync);

    public const string SendMessageAsync = nameof(SendMessageAsync);

    public const string ReceiveMessage = nameof(ReceiveMessage);
}
