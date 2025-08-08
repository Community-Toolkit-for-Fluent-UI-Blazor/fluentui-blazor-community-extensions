namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents read state of the message.
/// </summary>
public enum ChatMessageReadState
{
    /// <summary>
    /// The message unread by the users.
    /// </summary>
    Unread,

    /// <summary>
    /// A user read the message.
    /// </summary>
    Read,

    /// <summary>
    /// All users read the message.
    /// </summary>
    ReadByEveryone
}
