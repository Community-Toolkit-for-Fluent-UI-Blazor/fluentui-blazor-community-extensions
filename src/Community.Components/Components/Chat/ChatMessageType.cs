namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the type of the message.
/// </summary>
[Flags]
public enum ChatMessageType
{
    /// <summary>
    /// The message is undefined.
    /// </summary>
    /// <remarks>This state is not used.</remarks>
    None = 0,

    /// <summary>
    /// The message contains some text.
    /// </summary>
    Text = 1,

    /// <summary>
    /// The message contains some documents.
    /// </summary>
    /// <remarks>Document means : audio or video or image files, but pdf, doc, docx, pptx are documents too.</remarks>
    Document = 2,

    /// <summary>
    /// The message contains a gift.
    /// </summary>
    /// <remarks>Gift is defined by the user.</remarks>
    Gift = 4,
}
