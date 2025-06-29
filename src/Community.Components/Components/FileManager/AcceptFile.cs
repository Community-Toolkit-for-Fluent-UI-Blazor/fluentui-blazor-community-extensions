namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the accepted files inside a <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
[Flags]
public enum AcceptFile
    : ulong
{
    /// <summary>
    /// No file allowed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Audio files allowed.
    /// </summary>
    /// <remarks>Represents the audio/* tag.</remarks>
    Audio = 1,

    /// <summary>
    /// Image files allowed.
    /// </summary>
    /// <remarks>Represents the image/* tag.</remarks>
    Image = 2,

    /// <summary>
    /// Video files allowed.
    /// </summary>
    /// <remarks>Represents the video/* tag.</remarks>
    Video = 4,

    /// <summary>
    /// Pdf files allowed.
    /// </summary>
    /// <remarks>Represents the .pdf tag.</remarks>
    Pdf = 8,

    /// <summary>
    /// Excel files allowed.
    /// </summary>
    /// <remarks>Represents the .xls and .xlsx tag.</remarks>
    Excel = 16,

    /// <summary>
    /// Word files allowed.
    /// </summary>
    /// <remarks>Represents the .doc and .docx tag.</remarks>
    Word = 32,

    /// <summary>
    /// Powerpoint files allowed.
    /// </summary>
    /// <remarks>Represents the .ppt and .pptx tag.</remarks>
    Powerpoint = 64,

    /// <summary>
    /// Document files allowed.
    /// </summary>
    /// <remarks>Represents <see cref="Pdf"/>, <see cref="Word"/>, <see cref="Excel"/>, <see cref="Powerpoint"/> tags.</remarks>
    Document = Excel | Word | Powerpoint | Pdf
}
