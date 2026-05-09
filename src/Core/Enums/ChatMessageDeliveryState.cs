namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the delivery state of a chat message.
/// </summary>
public enum ChatMessageDeliveryState
{
    /// <summary>
    /// The message is not yet sent to the server.
    /// </summary>
    Pending,

    /// <summary>
    /// The message is sent to the server.
    /// </summary>
    Sent,

    /// <summary>
    /// The message was received by the client.
    /// </summary>
    Delivered,

    /// <summary>
    /// Failed during sending the message to the server.
    /// </summary>
    Failed
}
