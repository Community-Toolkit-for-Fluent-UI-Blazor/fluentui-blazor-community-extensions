namespace FluentUI.Blazor.Community.Components.Enums;

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
    /// <remarks>Files means : audio or video or image files, but pdf, doc, docx, pptx are files too.</remarks>
    Files = 2,

    /// <summary>
    /// The message contains a custom content.
    /// </summary>
    /// <remarks>Custom is defined by the user.</remarks>
    Custom = 4,
}
