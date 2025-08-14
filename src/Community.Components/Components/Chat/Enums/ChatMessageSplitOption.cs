namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Options to split the message.
/// </summary>
/// <remarks>This option is used to split text and documents into their own messages.</remarks>
public enum ChatMessageSplitOption
{
    /// <summary>
    /// The message is not split into multiple messages.
    /// </summary>
    None,

    /// <summary>
    /// The message (text + document) is split into multiples messages.
    /// </summary>
    Split
}
